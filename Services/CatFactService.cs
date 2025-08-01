using System.Net.Http.Json;
using CatfactCollector.Models;

namespace CatfactCollector.Services;

public class CatFactService : ICatFactService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<CatFactService> _logger;
    
    public CatFactService(HttpClient httpClient, ILogger<CatFactService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
        _httpClient.BaseAddress = new Uri("https://catfact.ninja/");
    }
    
    public async Task<CatFact?> GetRandomCatFactAsync()
    {
        try
        {
            var response = await _httpClient.GetFromJsonAsync<CatFactApiResponse>("fact");
            
            if (response == null)
                return null;
                
            return new CatFact
            {
                Id = Guid.NewGuid().ToString(),
                Fact = response.Fact,
                Length = response.Length,
                CreatedAt = DateTime.UtcNow
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching cat fact");
            return null;
        }
    }
    
    private class CatFactApiResponse
    {
        public string Fact { get; set; } = string.Empty;
        public int Length { get; set; }
    }
}

