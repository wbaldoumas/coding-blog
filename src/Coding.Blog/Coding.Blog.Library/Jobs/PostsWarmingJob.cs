using Coding.Blog.Library.Clients;
using Coding.Blog.Library.Records;
using Microsoft.Extensions.Logging;
using Quartz;

namespace Coding.Blog.Library.Jobs;

public sealed class PostsWarmingJob(ICosmicClient<CosmicPosts> postsClient, ILogger<PostsWarmingJob> logger) : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        logger.LogInformation("Warming posts cache...");

        await postsClient.GetAsync().ConfigureAwait(false);

        logger.LogInformation("Finished warming posts cache.");
    }
}
