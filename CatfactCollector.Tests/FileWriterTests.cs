using System;
using System.IO;
using System.Threading.Tasks;
using CatfactCollector.Configuration;
using CatfactCollector.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Xunit;

namespace CatfactCollector.Tests
{
    public class FileWriterTests
    {
        [Fact]
        public async Task AppendLineAsync_WritesLines_ToFile()
        {
            var dir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(dir);
            var path = Path.Combine(dir, "facts.txt");
            var opts = Options.Create(new FileWriterOptions { OutputPath = path });
            var logger = NullLogger<FileWriter>.Instance;

            var writer = new FileWriter(opts, logger);
            await writer.AppendLineAsync("A");
            await writer.AppendLineAsync("B");

            var lines = await File.ReadAllLinesAsync(path);
            Assert.Equal(new[] { "A", "B" }, lines);
        }
    }
}