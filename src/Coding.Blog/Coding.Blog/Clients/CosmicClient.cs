using Coding.Blog.DataTransfer;
using Coding.Blog.DataTransfer.PostProcessors;
using Coding.Blog.Library.Extensions;
using Coding.Blog.Options;
using Flurl;
using Flurl.Http;
using Microsoft.Extensions.Options;
using Polly;

namespace Coding.Blog.Clients;

internal sealed class CosmicClient<T>(
    IOptions<CosmicOptions> options,
    ILogger<T> logger,
    IAsyncPolicy<IEnumerable<T>> resiliencePolicy,
    ICosmicObjectPostProcessor<T> postProcessor
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

                    return postProcessor.Process(cosmicCollection.Objects);
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
