using Coding.Blog.Library.Configurations;
using Flurl;
using Flurl.Http;
using Microsoft.Extensions.Logging;
using Polly;

namespace Coding.Blog.Library.Clients;

public sealed class CosmicClient<T>(
    CosmicConfiguration configuration,
    ILogger<T> logger,
    IAsyncPolicy<T> resiliencePolicy
) : ICosmicClient<T>
{
    public async Task<T> GetAsync()
    {
        var typeName = typeof(T).FullName;
        var (type, props) = CosmicRequestRegistry.Requests[typeName!];
        var baseUrl = $"{configuration.Endpoint}/buckets/{configuration.BucketSlug}/objects";

        try
        {
            return await resiliencePolicy.ExecuteAsync(
                _ => baseUrl
                    .SetQueryParam("query", $"{{\"type\":\"{type}\"}}")
                    .SetQueryParam("read_key", configuration.ReadKey)
                    .SetQueryParam("props", props)
                    .GetJsonAsync<T>(),
                new Context(typeName)
            ).ConfigureAwait(false);
        }
        catch (Exception exception)
        {
            logger.LogError($"Failed to retrieve {typeName} from Cosmic API: {exception.Message}");

            throw;
        }
    }
}
