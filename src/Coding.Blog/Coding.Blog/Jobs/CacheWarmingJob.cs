using Coding.Blog.Clients;
using Coding.Blog.Library.Extensions;
using Quartz;

namespace Coding.Blog.Jobs;

internal sealed class CacheWarmingJob<T>(ICosmicClient<T> client, ILogger<CacheWarmingJob<T>> logger) : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        logger.LogCacheWarmingBegin(typeof(T).Name);

        await client.GetAsync().ConfigureAwait(false);

        logger.LogCacheWarmingEnd(typeof(T).Name);
    }
}
