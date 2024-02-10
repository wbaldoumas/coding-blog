using Coding.Blog.Clients;
using Coding.Blog.DataTransfer;
using Coding.Blog.Library.Protos;
using Coding.Blog.Library.Utilities;
using Coding.Blog.Services;
using FluentAssertions;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using NSubstitute;
using NUnit.Framework;

namespace Coding.Blog.Tests.Services;

[TestFixture]
internal sealed class PostsServiceTests
{
    private ICosmicClient<CosmicPost> _mockClient = null!;

    private IMapper _mockMapper = null!;

    private PostsService _postsService = null!;

    [SetUp]
    public void SetUp()
    {
        _mockClient = Substitute.For<ICosmicClient<CosmicPost>>();
        _mockMapper = Substitute.For<IMapper>();
        _postsService = new PostsService(_mockClient, _mockMapper);
    }

    [Test]
    public async Task WhenPostsServiceIsInvoked_ThenPostsAreReturned()
    {
        // arrange
        _mockClient
            .GetAsync()
            .Returns(ExpectedCosmicPosts);

        _mockMapper
            .Map<CosmicPost, Post>(ExpectedCosmicPosts)
            .Returns(ExpectedPosts);

        var request = new PostsRequest();
        var serverCallContext = Substitute.For<ServerCallContext>();

        // act
        var actualPosts = await _postsService.GetPosts(request, serverCallContext);

        // assert
        actualPosts.Posts.Should().BeEquivalentTo(ExpectedPosts);
    }

    private static readonly CosmicPost ExpectedCosmicPost = new(
        Id: "some id",
        Slug: "some-slug",
        DatePublished: DateTime.UtcNow,
        Title: "some title",
        Metadata: new CosmicPostMetadata(
            Description: "some description",
            Content: "some markdown",
            Image: new CosmicImage(
                Url: "some other url"
            ),
            Tags: "some,tags"
        )
    );

    private static readonly List<CosmicPost> ExpectedCosmicPosts = [ExpectedCosmicPost];

    private static readonly Post ExpectedPost = new()
    {
        Title = "some title",
        Description = "some description",
        Image = new Image
        {
            Url = "some other url"
        },
        Content = "some content",
        DatePublished = DateTime.UtcNow.ToTimestamp(),
        Id = "some id",
        ReadingTime = Duration.FromTimeSpan(TimeSpan.MaxValue),
        Slug = "some-slug"
    };

    private static readonly List<Post> ExpectedPosts = [ExpectedPost];
}
