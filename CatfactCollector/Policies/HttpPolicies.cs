// HttpPolicies.cs
using Polly;
using Polly.Extensions.Http;
using Polly.Timeout;
using System.Net;

namespace CatfactCollector.Policies
{
    public static class HttpPolicies
    {
        public static IAsyncPolicy<HttpResponseMessage> RetryJitter()
            => HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(r => r.StatusCode == HttpStatusCode.TooManyRequests)
                .WaitAndRetryAsync(new[]
                {
                    TimeSpan.FromMilliseconds(250),
                    TimeSpan.FromMilliseconds(500),
                    TimeSpan.FromSeconds(1)
                });

        public static IAsyncPolicy<HttpResponseMessage> TimeoutPerTry(int seconds = 5)
            => Policy.TimeoutAsync<HttpResponseMessage>(TimeSpan.FromSeconds(seconds));
    }
}