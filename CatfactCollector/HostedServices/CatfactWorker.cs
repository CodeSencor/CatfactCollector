using CatfactCollector.Configuration;
using CatfactCollector.Interfaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CatfactCollector.HostedServices;

public class CatfactWorker(ICatfactService catfactService, IFileWriter fileWriter, IOptions<CatfactWorkerOptions> opts, ILogger<CatfactWorker> logger) : BackgroundService
{
    private readonly int _intervalSeconds = opts.Value.IntervalSeconds;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation($"Starting {nameof(CatfactWorker)}");
        
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var fact = await catfactService.GetCatfactAsync();
                await fileWriter.AppendLineAsync(fact);
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                logger.LogInformation($"Cancellation requested, stopping {nameof(CatfactWorker)}");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"An error occurred in the {nameof(CatfactWorker)}");
            }

            await Task.Delay(TimeSpan.FromSeconds(_intervalSeconds), stoppingToken);
        }
        
        logger.LogInformation($"Stopping {nameof(CatfactWorker)}");
    }
}