using System.ComponentModel.DataAnnotations;

namespace CatfactCollector.Configuration;

public class CatfactServiceOptions
{
    [Url] public string BaseUrl { get; set; } = "https://catfact.ninja/";
    public string RelativePath { get; set; } = "fact";
    [Range(1, 60)] public int TimeoutSeconds { get; set; } = 5;
}