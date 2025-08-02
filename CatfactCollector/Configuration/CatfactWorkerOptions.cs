using System.ComponentModel.DataAnnotations;

namespace CatfactCollector.Configuration;

public class CatfactWorkerOptions
{
    [Range(1, 3600)] public int IntervalSeconds { get; set; } = 5;
}