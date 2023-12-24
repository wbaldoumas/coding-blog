using Coding.Blog.Shared.Clients;
using Coding.Blog.Shared.Records;
using Microsoft.Extensions.Logging;
using Quartz;

namespace Coding.Blog.Shared.Jobs;

public sealed class ProjectsWarmingJob(
    ICosmicClient<CosmicProjects> projectsClient,
    ILogger<ProjectsWarmingJob> logger
) : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        logger.LogInformation("Warming projects cache...");

        await projectsClient.GetAsync().ConfigureAwait(false);

        logger.LogInformation("Finished warming projects cache.");
    }
}
