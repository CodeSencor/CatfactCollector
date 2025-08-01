using CatfactCollector.Configuration;
using CatfactCollector.Interfaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace CatfactCollector.HostedServices;

public class CatfactWorker(ICatfactService catfactService, IFileWriter fileWriter, IOptions<CatfactWorkerOptions> opts) : BackgroundService
{
    private readonly int _intervalSeconds = opts.Value.IntervalSeconds;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var fact = await catfactService.GetCatfactAsync();
                await fileWriter.AppendLineAsync(fact);
            }
            catch (Exception ex)
            {
                //TODO Add logger logic    
            }

            await Task.Delay(TimeSpan.FromSeconds(_intervalSeconds), stoppingToken);
        }
    }
}