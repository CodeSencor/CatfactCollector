using System.Net.Http;
using CatfactCollector.Configuration;
using CatfactCollector.HostedServices;
using CatfactCollector.Interfaces;
using CatfactCollector.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace CatfactCollector;

class Program
{
    static async Task Main(string[] args)
    {
        var builder = Host.CreateDefaultBuilder();

        builder.ConfigureAppConfiguration(config =>
        {
            config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: false);
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