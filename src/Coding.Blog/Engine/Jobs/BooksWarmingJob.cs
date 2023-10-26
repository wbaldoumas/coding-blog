using Coding.Blog.Engine.Clients;
using Coding.Blog.Engine.Records;
using Microsoft.Extensions.Logging;
using Quartz;

namespace Coding.Blog.Engine.Jobs;

public sealed class BooksWarmingJob : IJob
{
    private readonly ICosmicClient<CosmicBooks> _booksClient;

    private readonly ILogger<BooksWarmingJob> _logger;

    public BooksWarmingJob(ICosmicClient<CosmicBooks> booksClient, ILogger<BooksWarmingJob> logger)
    {
        _booksClient = booksClient;
        _logger = logger;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        _logger.LogInformation("Warming books cache...");

        await _booksClient.GetAsync().ConfigureAwait(false);

        _logger.LogInformation("Finished warming books cache.");
    }
}