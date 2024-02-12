using Coding.Blog.DataTransfer;
using Coding.Blog.DataTransfer.PostProcessors;
using FluentAssertions;
using Markdig;
using NUnit.Framework;

namespace Coding.Blog.Tests.DataTransfer.PostProcessors;

[TestFixture]
internal sealed class CosmicPostsPostProcessorTests
{
    private MarkdownPipeline _mockMarkdownPipeline = null!;

    private CosmicPostsPostProcessor _cosmicPostsPostProcessor = null!;

    [SetUp]
    public void SetUp()
    {
        _mockMarkdownPipeline = new MarkdownPipelineBuilder().Build();
        _cosmicPostsPostProcessor = new CosmicPostsPostProcessor(_mockMarkdownPipeline);
    }

    [Test]
    public void Process_WithCosmicPosts_ReturnsProcessedCosmicPosts()
    {
        // arrange
        var cosmicPosts = new[]
        {
            new CosmicPost(
                Title: "Sample title",
                DatePublished: DateTime.MaxValue,
                Metadata: new CosmicPostMetadata(
                    Image: new CosmicImage(
                        Url: "https://example.com/sample-image"
                    ),
                    Tags: "Tag1, Tag2, Tag3",
                    Content: "Sample content",
                    Description: "Sample description"
                ),
                Id: "sample-id",
                Slug: "sample-slug"
            )
        };

        // act
        var result = _cosmicPostsPostProcessor.Process(cosmicPosts);

        // assert
        result.Should().BeEquivalentTo(
            new[]
            {
                new CosmicPost(
                    Title: "Sample title",
                    DatePublished: DateTime.MaxValue,
                    Metadata: new CosmicPostMetadata(
                        Image: new CosmicImage(
                            Url: "https://example.com/sample-image"
                        ),
                        Tags: "Tag1, Tag2, Tag3",
                        Content: "<p>Sample content</p>\n",
                        Description: "Sample description"
                    ),
                    Id: "sample-id",
                    Slug: "sample-slug"
                )
            }
        );
    }
}
