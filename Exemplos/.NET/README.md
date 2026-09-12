# Exemplo .NET

Requer o SDK .NET 8.

```bash
export LAMPADAS_API_URL=http://localhost:3000/api
export COD_EQUIPE=0
dotnet run --project Exemplos/.NET
```

Use `dotnet run --project Exemplos/.NET -- --repeat` para manter a sequência ativa durante a apresentação. O exemplo usa apenas `HttpClient` e `System.Net.Http.Json` da plataforma.
