# Servidor do painel

O servidor é uma aplicação Node.js sem dependências externas de runtime. Ele entrega o painel estático e mantém o estado da rodada em memória. As atualizações são publicadas para o navegador por **Server-Sent Events (SSE)** em `/api/events`.

## Executar

```bash
cd Servidor
npm start
```

O painel ficará disponível em `http://localhost:3000`. Para desenvolvimento, use `npm run dev`.

As variáveis opcionais são:

| Variável | Padrão | Uso |
| --- | --- | --- |
| `PORT` | `3000` | Porta HTTP |
| `LAMP_COUNT` | `10` | Quantidade de lâmpadas exibidas |
| `DEFAULT_TEAM_CODE` | `0` | Código da equipe inicial |
| `DEFAULT_TEAM_NAME` | `squad37` | Nome da equipe inicial |
| `CORS_ORIGIN` | `*` | Origem permitida para chamadas cross-origin |

## API compatível com os clientes de desafios

- `GET /api/lampada/status`: lista o estado das lâmpadas.
- `PUT /api/lampada/{id}/status`: recebe `{ "idEquipe": 0, "status": true }`.
- `PUT /api/lampada/status/limpar`: desliga todas as lâmpadas e reinicia o histórico.
- `GET /api/equipe`: lista as equipes.
- `POST /api/equipe`: cadastra uma equipe com `{ "codEquipe": 7, "nome": "Equipe 7" }`.
- `PUT /api/equipe/{codEquipe}`: altera nome e/ou autorização.
- `DELETE /api/equipe/{codEquipe}`: remove uma equipe.
- `GET /api/historico/exibir` e `/api/historico/ocultar`: controlam a exibição do histórico.
- `GET /api/events`: fluxo SSE usado pelo painel.
- `GET /health`: verificação de disponibilidade.

O estado é intencionalmente em memória porque o projeto serve como painel efêmero de campeonato. Reiniciar o processo inicia uma nova rodada.
