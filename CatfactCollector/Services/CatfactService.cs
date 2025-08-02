using System.Net.Http.Json;
using System.Text.Json;
using CatfactCollector.Configuration;
using CatfactCollector.Interfaces;
using CatfactCollector.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CatfactCollector.Services;

public class CatfactService(HttpClient client, IOptions<CatfactServiceOptions> opts, ILogger<CatfactService> logger) : ICatfactService
{
    private readonly string _endpointUrl = opts.Value.EndpointUrl;
    public async Task<string> GetCatfactAsync()
    {
        logger.LogInformation($"Fetching cat fact from {_endpointUrl}");

        try
        {
            var response = await client.GetFromJsonAsync<CatfactDto>(_endpointUrl);
            if (response?.fact == null)
            {
                logger.LogWarning($"Received null or empty fact from {_endpointUrl}");
                throw new JsonException("Fact not present or null");
            }

            logger.LogInformation($"Successfully fetched cat fact: {response.fact}");
            return response.fact;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"An error occurred while fetching cat fact from {_endpointUrl}");
            throw;
        }
    }
}