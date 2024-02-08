using Coding.Blog.Clients;
using Coding.Blog.Jobs;
using Coding.Blog.Library.Domain;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;
using Quartz;

namespace Coding.Blog.Tests.Jobs;

[TestFixture]
internal sealed class CacheWarmingJobTests
{
    private ICosmicClient<Book> _mockClient = null!;

    private ILogger<CacheWarmingJob<Book>> _mockLogger = null!;

    private CacheWarmingJob<Book> _cacheWarmingJob = null!;

    [SetUp]
    public void SetUp()
    {
        _mockClient = Substitute.For<ICosmicClient<Book>>();
        _mockLogger = Substitute.For<ILogger<CacheWarmingJob<Book>>>();
        _cacheWarmingJob = new CacheWarmingJob<Book>(_mockClient, _mockLogger);
    }

    [Test]
    public async Task WhenCacheWarmingJobIsInvoked_ThenCacheIsWarmed()
    {
        // arrange
        _mockClient.GetAsync().Returns(new List<Book>());

        // act
        await _cacheWarmingJob.Execute(Substitute.For<IJobExecutionContext>()).ConfigureAwait(false);

        // assert
        await _mockClient.Received(1).GetAsync().ConfigureAwait(false);
    }
}
