using Coding.Blog.Engine.Clients;
using Coding.Blog.Engine.Records;
using Microsoft.Extensions.Logging;
using Quartz;

namespace Coding.Blog.Engine.Jobs;

public sealed class PostsWarmingJob : IJob
{
    private readonly ICosmicClient<CosmicPosts> _postsClient;

    private readonly ILogger<PostsWarmingJob> _logger;

    public PostsWarmingJob(ICosmicClient<CosmicPosts> postsClient, ILogger<PostsWarmingJob> logger)
    {
        _postsClient = postsClient;
        _logger = logger;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        _logger.LogInformation("Warming posts cache...");

        await _postsClient.GetAsync().ConfigureAwait(false);

        _logger.LogInformation("Finished warming posts cache.");
    }
}