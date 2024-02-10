using Coding.Blog.Client.Clients;
using Coding.Blog.Client.Services;
using Coding.Blog.Client.Utilities;
using Coding.Blog.Library.Protos;
using FluentAssertions;
using Google.Protobuf.WellKnownTypes;
using NSubstitute;
using NUnit.Framework;
using Post = Coding.Blog.Library.Domain.Post;
using ProtoPost = Coding.Blog.Library.Protos.Post;

namespace Coding.Blog.Tests.Services;

[TestFixture]
internal sealed class ClientBlogServiceTests
{
    private IProtoClient<ProtoPost> _mockProtoClient = null!;

    private BlogService<ProtoPost, Post> _blogService = null!;

    [SetUp]
    public void SetUp()
    {
        _mockProtoClient = Substitute.For<IProtoClient<ProtoPost>>();

        _blogService = new BlogService<ProtoPost, Post>(_mockProtoClient, new Mapper());
    }

    [Test]
    public async Task WhenGetAsyncIsInvoked_ThenPostsAreReturned()
    {
        // arrange
        var protoPost = new ProtoPost
        {
            Slug = "example-slug",
            Content = "some content",
            Title = "some title",
            Description = "some description",
            Id = "some id",
            DatePublished = Timestamp.FromDateTime(DateTime.UtcNow),
            Image = new Image
            {
                Url = "some url"
            },
            Tags = "some,tags",
            ReadingTime = Duration.FromTimeSpan(TimeSpan.Zero)
        };

        _mockProtoClient.GetAsync()
            .Returns(new List<ProtoPost> { protoPost });

        // act
        var posts = await _blogService.GetAsync().ConfigureAwait(false);

        // assert
        posts.Should().ContainSingle(post =>
            protoPost.Slug.Equals(post.Slug, StringComparison.OrdinalIgnoreCase) &&
            protoPost.Id.Equals(post.Id, StringComparison.OrdinalIgnoreCase) &&
            protoPost.Content.Equals(post.Content, StringComparison.OrdinalIgnoreCase) &&
            protoPost.Description.Equals(post.Description, StringComparison.OrdinalIgnoreCase) &&
            protoPost.ReadingTime.ToTimeSpan().Equals(post.ReadingTime) &&
            protoPost.DatePublished.ToDateTime().Equals(post.DatePublished) &&
            protoPost.Image.Url.Equals(post.Image.Url, StringComparison.OrdinalIgnoreCase) &&
            protoPost.Title.Equals(post.Title, StringComparison.OrdinalIgnoreCase) &&
            protoPost.Tags.Equals(post.Tags, StringComparison.OrdinalIgnoreCase)
        );
    }
}
