using System.ComponentModel.DataAnnotations;

namespace CatfactCollector.Configuration;

public class FileWriterOptions
{
    [Required] public required string OutputPath { get; set; }
}