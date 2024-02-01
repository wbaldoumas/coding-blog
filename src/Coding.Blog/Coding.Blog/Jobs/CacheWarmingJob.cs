using Coding.Blog.Clients;
using Coding.Blog.Library.Extensions;
using Quartz;

namespace Coding.Blog.Jobs;

/// <summary>
///     A generic cache warming job for warming the cache of a <see cref="ICosmicClient{T}"/>.
/// </summary>
/// <typeparam name="T">The type of object to warm the cache for</typeparam>
/// <param name="client"> The <see cref="ICosmicClient{T}"/> to warm the cache for</param>
/// <param name="logger"> A <see cref="ILogger{T}"/> for logging</param>
internal sealed class CacheWarmingJob<T>(ICosmicClient<T> client, ILogger<CacheWarmingJob<T>> logger) : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        logger.LogCacheWarmingBegin(typeof(T).Name);

        await client.GetAsync().ConfigureAwait(false);

        logger.LogCacheWarmingEnd(typeof(T).Name);
    }
}
