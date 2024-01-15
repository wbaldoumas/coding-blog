using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Caching.Memory;
using Polly;
using Polly.Caching.Memory;
using Polly.Contrib.WaitAndRetry;

namespace Coding.Blog.Library.Utilities;

/// <summary>
///     Bundles the common resilience policies that I'd like to have in place at the
///     system boundaries (e.g. client-server communication, calls to 3rd-party APIs, etc.).
///
///     Returns a wait-and-retry policy wrapped in a memory-cache policy.
/// </summary>
[ExcludeFromCodeCoverage]
public static class ResiliencePolicyBuilder
{
    public static IAsyncPolicy<T> Build<T>(
        TimeSpan medianFirstRetryDelay,
        int retryCount,
        TimeSpan timeToLive)
    {
        var seed = Guid.NewGuid().GetHashCode();

        var sleepDurations = Backoff.DecorrelatedJitterBackoffV2(
            medianFirstRetryDelay,
            retryCount,
            seed
        );

        var waitAndRetry = Policy
            .Handle<Exception>()
            .WaitAndRetryAsync(sleepDurations);

        var memoryCacheOptions = new MemoryCacheOptions();
        var memoryCache = new MemoryCache(memoryCacheOptions);
        var memoryCacheProvider = new MemoryCacheProvider(memoryCache);

        var cache = Policy.CacheAsync<T>(memoryCacheProvider, timeToLive);

        var resiliencePolicy = cache.WrapAsync(waitAndRetry);

        return resiliencePolicy;
    }
}
