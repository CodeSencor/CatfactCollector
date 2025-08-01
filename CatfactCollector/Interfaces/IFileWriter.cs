namespace CatfactCollector.Interfaces;

public interface IFileWriter
{
    Task AppendLineAsync(string line);
}