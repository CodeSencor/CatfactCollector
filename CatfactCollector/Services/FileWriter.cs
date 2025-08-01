using CatfactCollector.Configuration;
using CatfactCollector.Interfaces;
using Microsoft.Extensions.Options;

namespace CatfactCollector.Services;

public class FileWriter(IOptions<FileWriterOptions> opts) : IFileWriter
{
    private readonly string _path = opts.Value.OutputPath;

    public Task AppendLineAsync(string line)
    {
        return File.AppendAllTextAsync(_path, line + Environment.NewLine);
    }
}