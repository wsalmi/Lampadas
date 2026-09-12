using System.Net.Http.Json;

namespace LampadasExample;

public sealed class LampadasClient : IDisposable
{
    private readonly HttpClient http;
    private readonly int teamCode;

    public LampadasClient()
    {
        var apiUrl = Environment.GetEnvironmentVariable("LAMPADAS_API_URL") ?? "http://localhost:3000/api";
        if (!Uri.TryCreate(apiUrl.TrimEnd('/'), UriKind.Absolute, out var baseUri))
            throw new InvalidOperationException("LAMPADAS_API_URL inválida.");

        if (!int.TryParse(Environment.GetEnvironmentVariable("COD_EQUIPE") ?? "0", out teamCode))
            throw new InvalidOperationException("COD_EQUIPE deve ser um número.");

        http = new HttpClient { BaseAddress = new Uri($"{baseUri}/") };
        http.Timeout = TimeSpan.FromSeconds(5);
    }

    public async Task<string> UpdateLampAsync(int lampId, bool status, CancellationToken cancellationToken = default)
    {
        var response = await http.PutAsJsonAsync($"lampada/{lampId}/status", new { idEquipe = teamCode, status }, cancellationToken);
        var body = await response.Content.ReadAsStringAsync(cancellationToken);
        response.EnsureSuccessStatusCode();
        return body;
    }

    public void Dispose() => http.Dispose();
}
