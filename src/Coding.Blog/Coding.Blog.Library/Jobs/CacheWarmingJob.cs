using Coding.Blog.Library.Clients;
using Microsoft.Extensions.Logging;
using Quartz;

namespace Coding.Blog.Library.Jobs;

public sealed class CacheWarmingJob<T>(ICosmicClient<T> client, ILogger<CacheWarmingJob<T>> logger) : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        logger.LogInformation("Warming {type} cache...", typeof(T).Name);

        await client.GetAsync().ConfigureAwait(false);

        logger.LogInformation("Finished warming {type} cache.", typeof(T).Name);
    }
}
