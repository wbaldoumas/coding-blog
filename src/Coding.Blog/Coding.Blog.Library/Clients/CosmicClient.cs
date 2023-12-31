﻿using Coding.Blog.Library.Options;
using Flurl;
using Flurl.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Polly;

namespace Coding.Blog.Library.Clients;

public sealed class CosmicClient<T>(
    IOptions<CosmicOptions> options,
    ILogger<T> logger,
    IAsyncPolicy<T> resiliencePolicy
) : ICosmicClient<T>
{
    public async Task<T> GetAsync()
    {
        var typeName = typeof(T).FullName;
        var (type, props) = CosmicRequestRegistry.Requests[typeName!];
        var baseUrl = $"{options.Value.Endpoint}/buckets/{options.Value.BucketSlug}/objects";

        try
        {
            return await resiliencePolicy.ExecuteAsync(
                _ => baseUrl
                    .SetQueryParam("query", $"{{\"type\":\"{type}\"}}")
                    .SetQueryParam("read_key", options.Value.ReadKey)
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