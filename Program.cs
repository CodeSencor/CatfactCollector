using CatfactCollector.Services;
using CatfactCollector.Workers;

var builder = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services.AddHttpClient<ICatFactService, CatFactService>();
        services.AddHostedService<CatFactWorker>();
    })
    .ConfigureLogging(logging =>
    {
        logging.ClearProviders();
        logging.AddConsole();
    });
    
var host = builder.Build();
await host.RunAsync();

