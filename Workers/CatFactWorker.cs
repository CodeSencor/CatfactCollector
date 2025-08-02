using CatfactCollector.Models;
using CatfactCollector.Services;

namespace CatfactCollector.Workers;

public class CatFactWorker : BackgroundService
{
    private readonly ICatFactService _catFactService;
    private readonly ILogger<CatFactWorker> _logger;
    private readonly List<CatFact> _collectedFacts = new();
    
    public CatFactWorker(ICatFactService catFactService, ILogger<CatFactWorker> logger)
    {
        _catFactService = catFactService;
        _logger = logger;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Cat Fact Worker starting at: {time}", DateTimeOffset.Now);
        
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var catFact = await _catFactService.GetRandomCatFactAsync();
                
                if (catFact != null)
                {
                    _collectedFacts.Add(catFact);
                    _logger.LogInformation("Cat Fact #{Count}: {Fact}", _collectedFacts.Count, catFact.Fact);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching cat fact");
            }
            
            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
        }
    }
}

