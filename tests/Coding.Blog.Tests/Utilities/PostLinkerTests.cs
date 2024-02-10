using Coding.Blog.Library.Domain;
using Coding.Blog.Library.Utilities;
using FluentAssertions;
using NUnit.Framework;

namespace Coding.Blog.Tests.Utilities;

[TestFixture]
internal sealed class PostLinkerTests
{
    private PostLinker _postLinker = null!;

    [SetUp]
    public void SetUp() => _postLinker = new PostLinker();

    [Test]
    public void Link_WithPosts_ReturnsLinkedPosts()
    {
        // arrange
        var now = DateTime.UtcNow;

        var post1 = CreateTestPost("1", now);
        var post2 = CreateTestPost("2", now.Subtract(TimeSpan.FromDays(1)));
        var post3 = CreateTestPost("3", now.Subtract(TimeSpan.FromDays(2)));

        var posts = new List<Post> { post1, post2, post3 };

        // act
        var result = _postLinker.Link(posts).ToList();

        // assert
        result.Should().NotBeNullOrEmpty();
        result.Should().HaveCount(3);

        result[0].Next.Should().BeNull();
        result[0].Previous!.Slug.Should().Be(post2.Slug);

        result[1].Next!.Slug.Should().Be(post1.Slug);
        result[1].Previous!.Slug.Should().Be(post3.Slug);

        result[2].Next!.Slug.Should().Be(post2.Slug);
        result[2].Previous.Should().BeNull();
    }

    private static Post CreateTestPost(string id, DateTime datePublished) => new(
        Id: id,
        Slug: $"sample-post-{id}",
        Title: $"Sample Title {id}",
        Content: $"Sample content {id}",
        Description: $"Sample description {id}",
        ReadingTime: TimeSpan.FromMinutes(5),
        DatePublished: datePublished,
        Tags: "Tag1, Tag2",
        Image: new Image(
            Url: $"https://example.com/image{id}.jpg"
        )
    );
}
