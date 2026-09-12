# Exemplos de clientes

Os exemplos mostram como um competidor pode chamar a API do painel usando diferentes linguagens. Todos usam o mesmo contrato:

```http
PUT /api/lampada/{idLampada}/status
Content-Type: application/json

{ "idEquipe": 7, "status": true }
```

## Configuração comum

- `LAMPADAS_API_URL`: URL base da API, por exemplo `http://localhost:3000/api`.
- `COD_EQUIPE`: código da equipe autorizada no painel.

Os programas executam **uma rodada por padrão**. Use a opção de repetição documentada em cada exemplo apenas quando quiser deixar o cliente rodando durante a apresentação. Isso evita loops infinitos acidentais e facilita testes locais.

| Exemplo | Execução |
| --- | --- |
| TypeScript/Node.js | `cd Solucao/Nodejs && npm ci && npm run once -- --task 1` |
| Python | `python3 Exemplos/Python/main.py --once` |
| Java | `cd Exemplos/Java && mvn test` e depois `mvn package` |
| .NET | `dotnet run --project Exemplos/.NET` |
