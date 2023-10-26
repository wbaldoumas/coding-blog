using Coding.Blog.Engine.Clients;
using Coding.Blog.Engine.Jobs;
using Coding.Blog.Engine.Records;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;

namespace Coding.Blog.UnitTests.Jobs;

[TestFixture]
internal sealed class PostsWarmingJobTests
{
    private ICosmicClient<CosmicPosts> _mockPostsClient = default!;

    private ILogger<PostsWarmingJob> _mockLogger = default!;

    private PostsWarmingJob _postsWarmingJob = default!;

    [SetUp]
    public void SetUp()
    {
        _mockPostsClient = Substitute.For<ICosmicClient<CosmicPosts>>();
        _mockLogger = Substitute.For<ILogger<PostsWarmingJob>>();

        _postsWarmingJob = new PostsWarmingJob(_mockPostsClient, _mockLogger);
    }

    [Test]
    public async Task PostsWarmingJob_calls_posts_client()
    {
        // act
        await _postsWarmingJob.Execute(default!).ConfigureAwait(false);

        // assert
        _mockLogger.Received(1).LogInformation("Warming posts cache...");

        await _mockPostsClient.Received(1).GetAsync().ConfigureAwait(false);

        _mockLogger.Received(1).LogInformation("Finished warming posts cache.");
    }
}