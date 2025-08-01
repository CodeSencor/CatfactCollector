using System.Net.Http.Json;
using System.Text.Json;
using CatfactCollector.Configuration;
using CatfactCollector.Interfaces;
using CatfactCollector.Models;
using Microsoft.Extensions.Options;

namespace CatfactCollector.Services;

public class CatfactService(HttpClient client, IOptions<CatfactServiceOptions> opts) : ICatfactService
{
    private readonly string _endpointUrl = opts.Value.EndpointUrl;
    public async Task<string> GetCatfactAsync()
    {
        var response = await client.GetFromJsonAsync<CatfactDto>(_endpointUrl);
        return response?.fact ?? throw new JsonException("Fact not present or null");
    }
}