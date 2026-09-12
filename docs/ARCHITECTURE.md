# Arquitetura e revisão técnica

## Contexto

O projeto é um painel efêmero para campeonatos de Hackathon. A equipe controla lâmpadas por HTTP e o público acompanha o estado em uma tela. O estado não precisa sobreviver a um reinício entre rodadas.

## Problemas encontrados na base anterior

| Área | Problema | Consequência |
| --- | --- | --- |
| Servidor | ASP.NET MVC/.NET Framework, SignalR, OWIN, Entity Framework e muitos pacotes antigos | Execução dependente de Visual Studio/IIS e difícil de reproduzir fora do Windows |
| Estado | Dicionários estáticos e objetos mutáveis alterados por vários controllers | Condições de corrida e regras de autorização espalhadas |
| API | Validação incompleta de equipe, lâmpada, nome e corpo JSON | Erros pouco previsíveis para os clientes |
| Interface | Bootstrap, jQuery, fontes e bibliotecas de terceiros vendorizadas em grande volume | Repositório pesado e alto acoplamento de assets |
| Clientes | URLs internas hardcoded, ambiente com nome incorreto e loops infinitos | Configuração manual frágil e risco de deixar processos rodando sem controle |
| Qualidade | Nenhum teste útil no servidor ou na solução TypeScript | Regressões difíceis de detectar |

## Decisões da reescrita

O servidor foi reduzido a Node.js 20+ com módulos nativos. A aplicação entrega o painel estático e a API no mesmo processo, eliminando configuração de IIS e dependências de runtime. O estado de uma rodada está encapsulado em `Servidor/src/store.js`; a camada HTTP em `Servidor/src/index.js` apenas traduz requisições para operações de domínio.

O painel usa HTML, CSS e JavaScript nativos. O navegador recebe o snapshot inicial e acompanha mudanças pelo endpoint SSE `/api/events`. SSE é suficiente para o fluxo unidirecional do servidor para a tela e evita o custo operacional do SignalR para este caso.

A API mantém os endpoints de controle usados historicamente por competidores. A alteração relevante é que respostas de erro agora usam `{ "error": { "code", "message" } }`, o que torna diagnósticos automáticos possíveis sem depender de texto livre.

A solução TypeScript foi reorganizada em quatro responsabilidades: `config.ts` valida o ambiente, `client.ts` trata HTTP, `tasks.ts` gera planos puros de operações e `runner.ts` executa os planos. A CLI em `index.ts` escolhe a tarefa e só repete quando `--repeat` é informado.

Os exemplos em Python, Java e .NET seguem o mesmo contrato, usam a URL definida por `LAMPADAS_API_URL`, informam `COD_EQUIPE` e têm uma rodada finita por padrão. Dependências de terceiros foram removidas dos exemplos .NET e Java sempre que a plataforma já oferece `HttpClient`.

## Regras de estado

1. Existe no máximo uma equipe autorizada.
2. Ao autorizar outra equipe, todas as lâmpadas são apagadas antes da troca.
3. Uma equipe não autorizada não pode alterar lâmpadas.
4. Uma lâmpada inexistente, equipe inexistente ou payload inválido resulta em erro HTTP explícito.
5. Reiniciar o servidor começa uma nova rodada, sem persistência acidental de dados anteriores.

## Validação

Os testes do servidor cobrem health check, snapshot, contrato de atualização, erro de equipe, erro de lâmpada, autorização e troca de equipe. Os testes do cliente cobrem parsing da CLI, configuração, Fibonacci, primos, aleatoriedade determinística, união/interseção e execução sequencial com espera.

```bash
cd Servidor && npm run check && npm test
cd ../Solucao/Nodejs && npm run check && npm test && npm run build
python3 -m py_compile ../Exemplos/Python/main.py ../Exemplos/Python/service/network.py
```
