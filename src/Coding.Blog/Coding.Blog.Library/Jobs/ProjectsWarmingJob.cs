using Coding.Blog.Library.Clients;
using Coding.Blog.Library.Records;
using Microsoft.Extensions.Logging;
using Quartz;

namespace Coding.Blog.Library.Jobs;

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
