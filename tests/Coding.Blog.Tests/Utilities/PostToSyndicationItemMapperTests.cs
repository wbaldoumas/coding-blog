using Coding.Blog.Library.Domain;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using System.Xml.Linq;
using Coding.Blog.Utilities;

namespace Coding.Blog.Tests.Utilities;

[TestFixture]
internal sealed class PostToSyndicationItemMapperTests
{
    private IStringSanitizer _mockStringSanitizer = null!;

    private PostToSyndicationItemMapper _postToSyndicationItemMapper = null!;

    [SetUp]
    public void SetUp()
    {
        _mockStringSanitizer = Substitute.For<IStringSanitizer>();
        _postToSyndicationItemMapper = new PostToSyndicationItemMapper(_mockStringSanitizer);
    }

    [Test]
    public void Map_WithPost_ReturnsSyndicationItem()
    {
        // arrange
        const string syndicationUrl = "https://example.com";

        var post = new Post(
            Id: "1",
            Slug: "sample-post",
            Title: "Sample Title",
            Content: "Sample content",
            Description: "Sample description",
            ReadingTime: TimeSpan.FromMinutes(5),
            DatePublished: DateTime.UtcNow,
            Tags: "Tag1, Tag2",
            Image: new Image(
                Url: "https://example.com/image.jpg",
                ImgixUrl: "https://example.com/image.jpg"
            )
        );

        var expectedPostUrl = new Uri(syndicationUrl + $"/post/{post.Slug}");
        const string expectedPostContent = "Sanitized content";
        var expectedPostDate = new DateTimeOffset(post.DatePublished);

        _mockStringSanitizer.Sanitize(post.Content).Returns(expectedPostContent);

        // act
        var result = _postToSyndicationItemMapper.Map(syndicationUrl, post);

        // assert
        result.Should().NotBeNull();

        result.Id.Should().Be(post.Id);
        result.Title.Text.Should().Be(post.Title);
        result.Links.Should().ContainSingle(link => link.Uri == expectedPostUrl && link.RelationshipType == "alternate");
        result.PublishDate.Should().Be(expectedPostDate);
        result.LastUpdatedTime.Should().Be(expectedPostDate);
        result.BaseUri.Should().Be(expectedPostUrl);
        result.ElementExtensions.Single().GetObject<XElement>().Value.Should().Be(post.Image.Url);
        result.Categories.Should().HaveCount(2);
        result.Categories.Should().Contain(category => category.Name == "Tag1");
        result.Categories.Should().Contain(category => category.Name == "Tag2");

        _mockStringSanitizer.Received(1).Sanitize(post.Description);
    }
}
