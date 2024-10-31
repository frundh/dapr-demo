namespace StateManagement;

class Program
{
  private static readonly Example[] Examples = new Example[]
  {
        new StateStoreExample(),
        new StateStoreTransactionsExample(),
        new StateStoreETagsExample(),
        new BulkStateExample()
  };

  static async Task<int> Main(string[] args)
  {
    if (args.Length > 0 && int.TryParse(args[0], out var index) && index >= 0 && index < Examples.Length)
    {
      var cts = new CancellationTokenSource();
      Console.CancelKeyPress += (object? sender, ConsoleCancelEventArgs e) => cts.Cancel();

      using (var httpClient = new HttpClient())
      {
        var daprSidecarUrl = "http://localhost:3500/v1.0/healthz";
        var isHealthy = false;
        while (!isHealthy)
        {
          try
          {
            var response = await httpClient.GetAsync(daprSidecarUrl, cts.Token);
            if (response.IsSuccessStatusCode)
            {
              isHealthy = true;
            }
            else
            {
              Console.WriteLine("Waiting for Dapr sidecar to be healthy...");
              await Task.Delay(1000, cts.Token);
            }
          }
          catch (HttpRequestException)
          {
            Console.WriteLine("Waiting for Dapr sidecar to be healthy...");
            await Task.Delay(1000, cts.Token);
          }
        }
      }

      await Examples[index].RunAsync(cts.Token);
      return 0;
    }

    Console.WriteLine("Hello, please choose a sample to run:");
    for (var i = 0; i < Examples.Length; i++)
    {
      Console.WriteLine($"{i}: {Examples[i].DisplayName}");
    }
    Console.WriteLine();
    return 1;
  }
}
