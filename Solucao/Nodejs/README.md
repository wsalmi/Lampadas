# Cliente TypeScript

Cliente de referência para os desafios de lógica do campeonato. A implementação não possui URL de servidor fixa e executa uma rodada única por padrão.

## Instalação

```bash
npm ci
cp .env.example .env
```

Configure `LAMPADAS_API_URL` e `COD_EQUIPE` no ambiente. O cliente também aceita `LAMP_COUNT`, `REQUEST_TIMEOUT_MS` e `DRY_RUN`.

## Uso

```bash
# uma tarefa específica
npm run once -- --task 1

# todas as tarefas, uma vez
npm run once -- --task all

# repetir até Ctrl+C, útil em demonstrações
npm run once -- --task all --repeat

# imprimir as operações sem fazer chamadas HTTP
npm run once -- --task 3 --dry-run
```

O identificador da tarefa vai de `1` a `8`. A ordem e as sequências estão em `src/tasks.ts` e são funções puras de planejamento, separadas do cliente HTTP para facilitar testes.
