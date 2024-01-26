﻿using Coding.Blog.Library.DataTransfer;
using Coding.Blog.Library.Extensions;
using Coding.Blog.Library.Options;
using Flurl;
using Flurl.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Polly;

namespace Coding.Blog.Library.Clients;

public sealed class CosmicClient<T>(
    IOptions<CosmicOptions> options,
    ILogger<T> logger,
    IAsyncPolicy<IEnumerable<T>> resiliencePolicy
) : ICosmicClient<T>
{
    public async Task<IEnumerable<T>> GetAsync()
    {
        var typeName = typeof(T).FullName;
        var (type, props) = CosmicRequestRegistry.Requests[typeName!];
        var baseUrl = $"{options.Value.Endpoint}/buckets/{options.Value.BucketSlug}/objects";

        try
        {
            return await resiliencePolicy.ExecuteAsync(async _ =>
                {
                    var cosmicCollection = await baseUrl
                        .SetQueryParam("query", $"{{\"type\":\"{type}\"}}")
                        .SetQueryParam("read_key", options.Value.ReadKey)
                        .SetQueryParam("props", props)
                        .GetJsonAsync<CosmicObjects<T>>()
                        .ConfigureAwait(false);

                    return cosmicCollection.Objects;
                },
                new Context(typeName)
            ).ConfigureAwait(false);
        }
        catch (Exception exception)
        {
            logger.LogCosmicApiError(typeName!, exception.Message);

            throw;
        }
    }
}
