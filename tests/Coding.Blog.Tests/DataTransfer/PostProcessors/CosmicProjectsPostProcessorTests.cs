using Coding.Blog.DataTransfer;
using Coding.Blog.DataTransfer.PostProcessors;
using FluentAssertions;
using Markdig;
using NUnit.Framework;

namespace Coding.Blog.Tests.DataTransfer.PostProcessors;

[TestFixture]
internal sealed class CosmicProjectsPostProcessorTests
{
    private MarkdownPipeline _mockMarkdownPipeline = null!;

    private CosmicProjectsPostProcessor _cosmicProjectsPostProcessor = null!;

    [SetUp]
    public void SetUp()
    {
        _mockMarkdownPipeline = new MarkdownPipelineBuilder().Build();
        _cosmicProjectsPostProcessor = new CosmicProjectsPostProcessor(_mockMarkdownPipeline);
    }

    [Test]
    public void Process_WithCosmicProjects_ReturnsProcessedCosmicProjects()
    {
        // arrange
        var cosmicProjects = new[]
        {
            new CosmicProject(
                Title: "Sample title",
                Metadata: new CosmicProjectMetadata(
                    Url: "https://example.com/sample-title",
                    Content: "Sample content",
                    Image: new CosmicImage(
                        Url: "https://example.com/sample-image"
                    ),
                    Rank: 1,
                    Tags: "Tag1, Tag2, Tag3"
                )
            )
        };

        // act
        var result = _cosmicProjectsPostProcessor.Process(cosmicProjects);

        // assert
        result.Should().BeEquivalentTo(
            new[]
            {
                new CosmicProject(
                    Title: "Sample title",
                    Metadata: new CosmicProjectMetadata(
                        Url: "https://example.com/sample-title",
                        Content: "<p>Sample content</p>\n",
                        Image: new CosmicImage(
                            Url: "https://example.com/sample-image"
                        ),
                        Rank: 1,
                        Tags: "Tag1, Tag2, Tag3"
                    )
                )
            }
        );
    }
}
