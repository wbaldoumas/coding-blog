using Coding.Blog.Engine;
using Coding.Blog.Engine.Clients;
using Coding.Blog.Engine.Mappers;
using Coding.Blog.Engine.Records;
using Coding.Blog.Engine.Services;
using FluentAssertions;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using NSubstitute;
using NUnit.Framework;
using Grpc.Core.Testing;

namespace Coding.Blog.UnitTests.Services;

[TestFixture]
internal sealed class PostsServiceTests
{
    private ICosmicClient<CosmicPosts> _mockCosmicPostClient = default!;
    private IMapper<CosmicPost, Post> _mockPostMapper = default!;
    private IPostLinker _mockPostLinker = default!;

    [SetUp]
    public void SetUp()
    {
        _mockCosmicPostClient = Substitute.For<ICosmicClient<CosmicPosts>>();
        _mockPostMapper = Substitute.For<IMapper<CosmicPost, Post>>();
        _mockPostLinker = Substitute.For<IPostLinker>();
    }

    [Test]
    public async Task PostsService_generates_expected_response_when_posts_exist()
    {
        // arrange
        var mockDatePublished = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);

        var mockCosmicPostsClientResponse = new CosmicPosts(
            new List<CosmicPost>
            {
                new(
                    "12345",
                    "test-post-a",
                    "Test Post A",
                    mockDatePublished,
                    new CosmicPostMetadata(
                        new CosmicPostHero("https://google.com/a", "https://google.com/b"), 
                        string.Empty,
                        "This is some markdown talking about Test Post A"
                        )
                ),
                new(
                    "123456",
                    "test-post-b",
                    "Test Post B",
                    mockDatePublished,
                    new CosmicPostMetadata(
                        new CosmicPostHero("https://google.com/b", "https://google.com/c"), 
                        string.Empty,
                        "This is some markdown talking about Test Post B"
                        )
                )
            }
        );

        _mockCosmicPostClient
            .GetAsync()
            .Returns(mockCosmicPostsClientResponse);

        var mockPostMapperResponse = mockCosmicPostsClientResponse.Posts.Select(source => new Post
        {
            Id = source.Id,
            Slug = source.Slug,
            Title = source.Title,
            Content = source.Metadata.Markdown,
            DatePublished = Timestamp.FromDateTime(source.DatePublished),
            Hero = new Hero
            {
                Url = source.Metadata.Hero.Url,
                ImgixUrl = source.Metadata.Hero.ImgixUrl
            }
        }).ToList();

        _mockPostMapper
            .Map(mockCosmicPostsClientResponse.Posts)
            .Returns(mockPostMapperResponse);

        var mockPostLinkerResponse = mockPostMapperResponse.Select(post => new Post(post)
        {
            Next = new Post(),
            Previous = new Post()
        }).ToList();

        _mockPostLinker
            .Link(mockPostMapperResponse)
            .Returns(mockPostLinkerResponse);

        var mockServerCallContext = TestServerCallContext.Create(
            "GetPosts",
            "TestHost",
            DateTime.MaxValue,
            Metadata.Empty,
            CancellationToken.None,
            "TestPeer",
            new AuthContext("TestPeerIdentity", new Dictionary<string, List<AuthProperty>>(StringComparer.OrdinalIgnoreCase)),
            null,
            _ => Task.CompletedTask,
            () => new WriteOptions(),
            _ => { }
        );

        var service = new PostsService(_mockCosmicPostClient, _mockPostMapper, _mockPostLinker);

        // act
        var response = await service.GetPosts(new PostsRequest(), mockServerCallContext);

        // assert
        response.Should().NotBeNull();
        response.Posts.Should().HaveCount(2);

        await _mockCosmicPostClient
            .Received(1)
            .GetAsync();

        _mockPostMapper
            .Received(1)
            .Map(mockCosmicPostsClientResponse.Posts);

        _mockPostLinker
            .Received(1)
            .Link(mockPostMapperResponse);
    }

    [Test]
    public async Task PostsService_generates_expected_response_when_posts_do_not_exist()
    {
        // arrange
        var mockCosmicPostsClientResponse = new CosmicPosts(new List<CosmicPost>());

        _mockCosmicPostClient
            .GetAsync()
            .Returns(mockCosmicPostsClientResponse);

        // ReSharper disable once CollectionNeverUpdated.Local
        var mockPostMapperResponse = new List<Post>();

        _mockPostMapper
            .Map(mockCosmicPostsClientResponse.Posts)
            .Returns(mockPostMapperResponse);

        _mockPostLinker
            .Link(mockPostMapperResponse)
            .Returns(new List<Post>());

        var mockServerCallContext = TestServerCallContext.Create(
            "GetPosts",
            "TestHost",
            DateTime.MaxValue,
            Metadata.Empty,
            CancellationToken.None,
            "TestPeer",
            new AuthContext("TestPeerIdentity", new Dictionary<string, List<AuthProperty>>(StringComparer.OrdinalIgnoreCase)),
            null,
            _ => Task.CompletedTask,
            () => new WriteOptions(),
            _ => { }
        );

        var service = new PostsService(_mockCosmicPostClient, _mockPostMapper, _mockPostLinker);

        // act
        var response = await service.GetPosts(new PostsRequest(), mockServerCallContext);

        // assert
        response.Should().NotBeNull();
        response.Posts.Should().BeEmpty();

        await _mockCosmicPostClient
            .Received(1)
            .GetAsync();

        _mockPostMapper
            .Received(1)
            .Map(mockCosmicPostsClientResponse.Posts);

        _mockPostLinker
            .Received(1)
            .Link(mockPostMapperResponse);
    }
}