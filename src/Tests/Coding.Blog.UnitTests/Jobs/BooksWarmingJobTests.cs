using Coding.Blog.Engine.Clients;
using Coding.Blog.Engine.Jobs;
using Coding.Blog.Engine.Records;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;

namespace Coding.Blog.UnitTests.Jobs;

[TestFixture]
internal sealed class BooksWarmingJobTests
{
    private ICosmicClient<CosmicBooks> _mockBooksClient = default!;

    private ILogger<BooksWarmingJob> _mockLogger = default!;

    private BooksWarmingJob _booksWarmingJob = default!;

    [SetUp]
    public void SetUp()
    {
        _mockBooksClient = Substitute.For<ICosmicClient<CosmicBooks>>();
        _mockLogger = Substitute.For<ILogger<BooksWarmingJob>>();

        _booksWarmingJob = new BooksWarmingJob(_mockBooksClient, _mockLogger);
    }

    [Test]
    public async Task BooksWarmingJob_calls_books_client()
    {
        // act
        await _booksWarmingJob.Execute(default!).ConfigureAwait(false);

        // assert
        _mockLogger.Received(1).LogInformation("Warming books cache...");

        await _mockBooksClient.Received(1).GetAsync().ConfigureAwait(false);

        _mockLogger.Received(1).LogInformation("Finished warming books cache.");
    }
}