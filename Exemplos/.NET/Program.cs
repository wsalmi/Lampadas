namespace LampadasExample;

public static class Program
{
    public static async Task Main(string[] args)
    {
        var repeat = args.Contains("--repeat", StringComparer.OrdinalIgnoreCase);
        var delay = int.TryParse(Environment.GetEnvironmentVariable("DELAY_MS"), out var configuredDelay)
            ? Math.Max(0, configuredDelay)
            : 500;

        using var client = new LampadasClient();
        do
        {
            foreach (var lampId in Enumerable.Range(1, 10))
            {
                Console.WriteLine(await client.UpdateLampAsync(lampId, false));
                await Task.Delay(delay);
                Console.WriteLine(await client.UpdateLampAsync(lampId, true));
                await Task.Delay(delay);
            }
        } while (repeat);
    }
}
