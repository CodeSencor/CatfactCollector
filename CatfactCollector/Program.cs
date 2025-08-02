using System.Net.Http;
using CatfactCollector.Configuration;
using CatfactCollector.HostedServices;
using CatfactCollector.Interfaces;
using CatfactCollector.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CatfactCollector;

class Program
{
    static async Task Main(string[] args)
    {
        var builder = Host.CreateDefaultBuilder(args);
        
        builder.ConfigureAppConfiguration(config =>
        {
            config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: false);
            config.AddCommandLine(source =>
            {
                source.Args = args;
                source.SwitchMappings = new Dictionary<string, string>()
                {
                    { "-l", "Logging:LogLevel:Default" },
                    { "--loglevel", "Logging:LogLevel:Default" }
                };
            });
        });

        builder.ConfigureLogging((context, logging) =>
        {
            logging.ClearProviders();
            
            logging.AddConfiguration(context.Configuration.GetSection("Logging"));
            
            logging.AddConsole();
        });
        
        builder.ConfigureServices((context, services) =>
        {
            
            
            services.Configure<FileWriterOptions>(context.Configuration.GetSection("FileWriter"));
            services.Configure<CatfactServiceOptions>(context.Configuration.GetSection("CatfactService"));
            services.Configure<CatfactWorkerOptions>(context.Configuration.GetSection("CatfactWorker"));
            
            services.AddHttpClient<ICatfactService, CatfactService>();
            services.AddSingleton<IFileWriter, FileWriter>();
            services.AddHostedService<CatfactWorker>();
        });

        await builder.Build().RunAsync();
    }
}