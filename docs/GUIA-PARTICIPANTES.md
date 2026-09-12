# Guia rápido dos participantes

Este guia mostra como sua solução deve enviar comandos para o painel de lâmpadas durante o Hackathon. Você não precisa conhecer a implementação do painel: basta fazer requisições HTTP para a URL fornecida pela organização.

## 1. Configure a conexão

A organização deve informar duas variáveis:

| Variável | Exemplo | Descrição |
| --- | --- | --- |
| `LAMPADAS_API_URL` | `http://localhost:3000/api` | URL base da API, sem `/lampada` no final |
| `COD_EQUIPE` | `7` | Código numérico da sua equipe |

Se estiver testando o painel localmente, a URL padrão é `http://localhost:3000/api`.

> Não coloque `/api/lampada` na variável `LAMPADAS_API_URL`. O cliente acrescenta esse caminho automaticamente.

## 2. Envie o estado de uma lâmpada

Use o endpoint abaixo sempre que sua lógica decidir que uma lâmpada deve acender ou apagar:

```http
PUT {LAMPADAS_API_URL}/lampada/{idLampada}/status
Content-Type: application/json

{
  "idEquipe": 7,
  "status": true
}
```

Os valores aceitos são:

- `idLampada`: inteiro entre `1` e `10`, salvo se a organização informar outra quantidade;
- `idEquipe`: o código numérico recebido pela equipe;
- `status`: `true` para acender e `false` para apagar.

Uma resposta bem-sucedida tem status HTTP `200` e formato semelhante a:

```json
{
  "lampada": 3,
  "status": "Acesa",
  "lampadaData": {
    "idLampada": 3,
    "status": true,
    "ultimaAlteracao": "2026-09-12T19:00:00.000Z"
  }
}
```

## 3. Teste pelo terminal

Com o servidor rodando e a equipe `7` autorizada, teste uma lâmpada com `curl`:

```bash
export LAMPADAS_API_URL=http://localhost:3000/api
export COD_EQUIPE=7

curl --fail-with-body -X PUT \
  "$LAMPADAS_API_URL/lampada/3/status" \
  -H 'Content-Type: application/json' \
  -d '{"idEquipe":7,"status":true}'

curl --fail-with-body -X PUT \
  "$LAMPADAS_API_URL/lampada/3/status" \
  -H 'Content-Type: application/json' \
  -d '{"idEquipe":7,"status":false}'
```

Para consultar o estado atual de todas as lâmpadas:

```bash
curl "$LAMPADAS_API_URL/lampada/status"
```

Para verificar se o servidor está disponível:

```bash
curl http://localhost:3000/health
```

## 4. Exemplo em JavaScript/Node.js

O ponto mais importante é aguardar cada requisição antes de enviar a próxima, para que a ordem da sua lógica seja preservada:

```js
const apiUrl = (process.env.LAMPADAS_API_URL ?? 'http://localhost:3000/api').replace(/\/+$/, '');
const teamCode = Number(process.env.COD_EQUIPE ?? 7);

async function atualizarLampada(idLampada, status) {
  const response = await fetch(`${apiUrl}/lampada/${idLampada}/status`, {
    method: 'PUT',
    headers: { 'content-type': 'application/json' },
    body: JSON.stringify({ idEquipe: teamCode, status }),
  });

  if (!response.ok) {
    throw new Error(`API retornou HTTP ${response.status}: ${await response.text()}`);
  }

  return response.json();
}

const esperar = (ms) => new Promise((resolve) => setTimeout(resolve, ms));

(async () => {
  for (const id of [1, 2, 3, 4, 5]) {
    await atualizarLampada(id, true);
    await esperar(500);
    await atualizarLampada(id, false);
  }
})().catch((error) => {
  console.error(error);
  process.exitCode = 1;
});
```

O `fetch` acima está disponível nativamente no Node.js 18 ou superior. Para projetos TypeScript, o cliente de referência em [`Solucao/Nodejs`](../Solucao/Nodejs/README.md) já implementa configuração, timeout, tratamento de erros, tarefas e modo `--dry-run`.

## 5. Exemplo em Python

```python
import os
import time
import requests

api_url = os.getenv("LAMPADAS_API_URL", "http://localhost:3000/api")
team_code = int(os.getenv("COD_EQUIPE", "7"))


def atualizar_lampada(id_lampada: int, status: bool) -> dict:
    response = requests.put(
        f"{api_url}/lampada/{id_lampada}/status",
        json={"idEquipe": team_code, "status": status},
        timeout=5,
    )
    response.raise_for_status()
    return response.json()


for id_lampada in [1, 2, 3, 4, 5]:
    atualizar_lampada(id_lampada, True)
    time.sleep(0.5)
    atualizar_lampada(id_lampada, False)
```

## 6. Erros mais comuns

| HTTP | Código | Causa | Como corrigir |
| --- | --- | --- | --- |
| `403` | `TEAM_NOT_AUTHORIZED` | Sua equipe existe, mas não é a equipe autorizada no momento | Confirme o código recebido e aguarde a autorização da organização |
| `404` | `TEAM_NOT_FOUND` | O código enviado não está cadastrado | Confira `COD_EQUIPE` e `idEquipe` |
| `404` | `LAMP_NOT_FOUND` | O identificador da lâmpada não existe | Use um ID dentro do intervalo informado |
| `400` | `BAD_REQUEST` | JSON, tipo ou valor inválido | Envie `idEquipe` numérico e `status` booleano |
| `409` | `TEAM_EXISTS` | Tentativa de cadastrar uma equipe já existente | Não cadastre equipes durante o desafio; essa etapa é da organização |

O corpo de uma resposta de erro segue este formato:

```json
{
  "error": {
    "code": "TEAM_NOT_AUTHORIZED",
    "message": "A equipe não está autorizada para controlar as lampadas"
  }
}
```

## 7. Regras importantes da competição

A equipe só pode controlar lâmpadas quando o código enviado coincide com a equipe autorizada pelo painel. Quando a organização troca a equipe autorizada, todas as lâmpadas são apagadas para iniciar uma nova rodada. O estado do painel é mantido em memória, portanto não dependa de informações de uma execução anterior.

Sua aplicação deve controlar o ritmo das chamadas. Envie somente as transições necessárias, aguarde cada resposta e use pequenos intervalos quando o desafio exigir uma animação. Não tente controlar a interface do navegador nem use o endpoint SSE `/api/events`; esse fluxo é destinado ao painel de apresentação.

## Checklist antes de apresentar

1. A URL da API está correta e acessível.
2. `COD_EQUIPE` contém um número, não uma string vazia.
3. Sua solução envia `status` como booleano.
4. Sua solução aguarda a resposta de cada `PUT`.
5. Você testou pelo menos uma lâmpada acesa e apagada.
6. Sua lógica trata erros HTTP e não entra em repetição infinita por acidente.
