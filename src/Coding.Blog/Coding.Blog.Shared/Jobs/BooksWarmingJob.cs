﻿using Coding.Blog.Shared.Clients;
using Coding.Blog.Shared.Records;
using Microsoft.Extensions.Logging;
using Quartz;

namespace Coding.Blog.Shared.Jobs;

public sealed class BooksWarmingJob(ICosmicClient<CosmicBooks> booksClient, ILogger<BooksWarmingJob> logger) : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        logger.LogInformation("Warming books cache...");

        await booksClient.GetAsync().ConfigureAwait(false);

        logger.LogInformation("Finished warming books cache.");
    }
}
