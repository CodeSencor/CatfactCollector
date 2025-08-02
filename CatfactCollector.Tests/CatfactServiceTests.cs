using System.Net;
using CatfactCollector.Configuration;
using CatfactCollector.Services;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;

namespace CatfactCollector.Tests;

class FakeHandler(HttpResponseMessage resp) : HttpMessageHandler
{
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage req, CancellationToken ct)
        => Task.FromResult(resp);
}

public class CatfactServiceTests
{
    [Fact]
    public async Task GetCatfactAsync_ReturnsFact_StatusOk()
    {
        // Arrange
        var factJson = "{\"fact\":\"Cats sleep a lot\", \"length\":16}";
        var handler = new FakeHandler(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(factJson)
        });
        var http = new HttpClient(handler) { BaseAddress = new Uri("https://catfact.ninja/") };

        var options = Options.Create(new CatfactServiceOptions { BaseUrl = "https://catfact.ninja/", RelativePath = "fact", TimeoutSeconds = 5 });
        var logger = NullLogger<CatfactService>.Instance;

        var svc = new CatfactService(http, options, logger);

        var fact = await svc.GetCatfactAsync();

        Assert.Equal("Cats sleep a lot", fact);
    }

    [Fact]
    public async Task GetCatfactAsync_Throws_ServerError()
    {
        var handler = new FakeHandler(new HttpResponseMessage(HttpStatusCode.InternalServerError));
        var http = new HttpClient(handler) { BaseAddress = new Uri("https://catfact.ninja/") };
        var options = Options.Create(new CatfactServiceOptions { BaseUrl = "https://catfact.ninja/", RelativePath = "fact", TimeoutSeconds = 5 });
        var logger = NullLogger<CatfactService>.Instance;

        var svc = new CatfactService(http, options, logger);

        await Assert.ThrowsAsync<HttpRequestException>(async () => await svc.GetCatfactAsync());
    }
}