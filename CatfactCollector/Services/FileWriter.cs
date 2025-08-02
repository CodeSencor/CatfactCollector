using CatfactCollector.Configuration;
using CatfactCollector.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CatfactCollector.Services;

public class FileWriter(IOptions<FileWriterOptions> opts, ILogger<FileWriter> logger) : IFileWriter
{
    private readonly string _path = opts.Value.OutputPath;

    public async Task AppendLineAsync(string line)
    {
        logger.LogInformation($"Appending line \"{line}\" to file at {_path}");

        try
        {
            await File.AppendAllTextAsync(_path, line + Environment.NewLine);
            logger.LogInformation("Successfully appended line to file");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"An error occurred while appending line to file at {_path}");
            throw;
        }
    }
}