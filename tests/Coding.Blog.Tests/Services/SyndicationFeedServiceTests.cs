using Coding.Blog.Library.Domain;
using Coding.Blog.Library.Services;
using Coding.Blog.Options;
using Coding.Blog.Services;
using FluentAssertions;
using Markdig;
using Microsoft.Extensions.Options;
using NSubstitute;
using NUnit.Framework;
using System.Xml.Linq;
using Coding.Blog.Utilities;
using OptionsBuilder = Microsoft.Extensions.Options.Options;

namespace Coding.Blog.Tests.Services;

[TestFixture]
internal sealed class SyndicationFeedServiceTests
{
    private IOptions<ApplicationInfoOptions> _mockAppInfoOptions = null!;

    private IBlogService<Post> _mockBlogPostService = null!;

    private IPostToSyndicationItemMapper _mapper = null!;

    private SyndicationFeedService _syndicationFeedService = null!;

    [SetUp]
    public void SetUp()
    {
        _mockAppInfoOptions = OptionsBuilder.Create(new ApplicationInfoOptions
        {
            Title = "Test Blog",
            Description = "Test Blog Description"
        });

        _mockBlogPostService = Substitute.For<IBlogService<Post>>();

        var markdownPipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().Build();
        var stringSanitizer = new StringSanitizer(markdownPipeline);

        _mapper = new PostToSyndicationItemMapper(stringSanitizer);
        _syndicationFeedService = new SyndicationFeedService(_mockAppInfoOptions, _mockBlogPostService, _mapper);
    }

    [Test]
    public async Task GetSyndicationFeed_ReturnsValidFeed()
    {
        // arrange
        const string syndicationUrl = "https://example.com/syndication";

        var posts = new List<Post>
        {
            new(
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
            )
        };

        _mockBlogPostService.GetAsync().Returns(Task.FromResult(posts as IEnumerable<Post>));

        // act
        var result = await _syndicationFeedService.GetSyndicationFeed(syndicationUrl).ConfigureAwait(false);

        // assert
        result.Should().NotBeNull();

        result.Title.Text.Should().Be("Test Blog");
        result.Description.Text.Should().Be("Test Blog Description");
        result.BaseUri.Should().Be(new Uri(syndicationUrl));
        result.LastUpdatedTime.Should().Be(new DateTimeOffset(posts.Max(post => post.DatePublished)));
        result.Links.Should().ContainSingle(link => link.Uri == new Uri(syndicationUrl) && link.RelationshipType == "alternate");

        // assert on items
        result.Items.Should().ContainSingle();
        result.Items.Single().Id.Should().Be(posts.Single().Id);
        result.Items.Single().Title.Text.Should().Be(posts.Single().Title);
        result.Items.Single().Links.Should().ContainSingle(link => link.Uri == new Uri(syndicationUrl + $"/post/{posts.Single().Slug}") && link.RelationshipType == "alternate");
        result.Items.Single().PublishDate.Should().Be(new DateTimeOffset(posts.Single().DatePublished));
        result.Items.Single().LastUpdatedTime.Should().Be(new DateTimeOffset(posts.Single().DatePublished));
        result.Items.Single().BaseUri.Should().Be(new Uri(syndicationUrl + $"/post/{posts.Single().Slug}"));
        result.Items.Single().ElementExtensions.Should().ContainSingle();
        result.Items.Single().ElementExtensions.Single().OuterName.Should().Be("image");
        result.Items.Single().ElementExtensions.Single().GetObject<XElement>().Value.Should().Be(posts.Single().Image.Url);
        result.Items.Single().Categories.Should().HaveCount(2);
        result.Items.Single().Categories.Should().ContainSingle(category => category.Name == "Tag1");
        result.Items.Single().Categories.Should().ContainSingle(category => category.Name == "Tag2");
    }
}
