using CatfactCollector.Configuration;
using CatfactCollector.HostedServices;
using CatfactCollector.Interfaces;
using CatfactCollector.Services;
using CatfactCollector.Policies;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CatfactCollector;

class Program
{
    static async Task Main(string[] args)
    {
        var builder = Host.CreateDefaultBuilder(args);

        builder.ConfigureAppConfiguration(config =>
        {
            config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: false);
            config.AddCommandLine(source =>
            {
                source.Args = args;
                source.SwitchMappings = new Dictionary<string, string>()
                {
                    { "-l", "Logging:LogLevel:Default" },
                    { "--loglevel", "Logging:LogLevel:Default" },

                    { "-o", "FileWriter:OutputPath" },
                    { "--output", "FileWriter:OutputPath" },

                    { "-e", "CatfactService:BaseUrl" },
                    { "--endpoint", "CatfactService:BaseUrl" },
        
                    { "-p", "CatfactService:RelativePath" },
                    { "--path", "CatfactService:RelativePath" },

                    { "-t", "CatfactService:TimeoutSeconds" },
                    { "--timeout", "CatfactService:TimeoutSeconds" },

                    { "-i", "CatfactWorker:IntervalSeconds" },
                    { "--interval", "CatfactWorker:IntervalSeconds" }
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

            services.AddOptions<FileWriterOptions>()
                .Bind(context.Configuration.GetSection("FileWriter"))
                .ValidateDataAnnotations()
                .ValidateOnStart();
            
            services.AddOptions<CatfactServiceOptions>()
                .Bind(context.Configuration.GetSection("CatfactService"))
                .ValidateDataAnnotations()
                .ValidateOnStart();

            services.AddHttpClient<ICatfactService, CatfactService>((sp, http) =>
                {
                    var o = sp.GetRequiredService<IOptions<CatfactServiceOptions>>().Value;
                    http.BaseAddress = new Uri(o.BaseUrl, UriKind.Absolute);
                    http.Timeout = TimeSpan.FromSeconds(o.TimeoutSeconds);
                })
                .AddPolicyHandler(HttpPolicies.RetryJitter())
                .AddPolicyHandler(HttpPolicies.TimeoutPerTry());

            services.AddSingleton<IFileWriter, FileWriter>();
            services.AddHostedService<CatfactWorker>();
        });

        await builder.Build().RunAsync();
    }
}