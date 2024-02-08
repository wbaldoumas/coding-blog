using Coding.Blog.Client.Clients;
using Coding.Blog.Library.Protos;
using FluentAssertions;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using NSubstitute;
using NUnit.Framework;

namespace Coding.Blog.Tests.Clients;

[TestFixture]
internal sealed class PostsClientTests
{
    private Posts.PostsClient _mockProtoPostsClient = null!;

    private PostsClient _postsClient = null!;

    [SetUp]
    public void SetUp()
    {
        _mockProtoPostsClient = Substitute.For<Posts.PostsClient>();
        _postsClient = new PostsClient(_mockProtoPostsClient);
    }

    [Test]
    public async Task WhenClientIsInvoked_ThenPostsAreReturned()
    {
        // arrange
        using var postsResponse = new AsyncUnaryCall<PostsReply>(
            Task.FromResult(new PostsReply
                {
                    Posts = { ExpectedPosts }
                }
            ),
            Task.FromResult(Metadata.Empty),
            () => Status.DefaultSuccess,
            () => Metadata.Empty,
            () => { }
        );

        _mockProtoPostsClient.GetPostsAsync(Arg.Any<PostsRequest>()).Returns(postsResponse);

        // act
        var actualPosts = await _postsClient.GetAsync().ConfigureAwait(false);

        // assert
        actualPosts.Should().BeEquivalentTo(ExpectedPosts);
    }

    private static readonly Post ExpectedPost = new()
    {
        Id = "some id",
        Title = "some title",
        Content = "some content",
        DatePublished = DateTime.UtcNow.ToTimestamp(),
        Description = "some description",
        Image = new Image
        {
            Url = "some url",
            ImgixUrl = "some other url"
        },
        Slug = "some-slug",
        ReadingTime = Duration.FromTimeSpan(TimeSpan.Zero),
        Tags = "some,tags"
    };

    private static readonly List<Post> ExpectedPosts = [ExpectedPost];
}
