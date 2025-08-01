namespace CatfactCollector.Models;

public class CatFact
{
    public string Id { get; set; } = string.Empty;
    public string Fact { get; set; } = string.Empty;
    public int Length { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

