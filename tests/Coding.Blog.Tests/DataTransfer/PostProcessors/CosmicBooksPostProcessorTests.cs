using Coding.Blog.DataTransfer;
using Coding.Blog.DataTransfer.PostProcessors;
using FluentAssertions;
using Markdig;
using NUnit.Framework;

namespace Coding.Blog.Tests.DataTransfer.PostProcessors;

[TestFixture]
internal sealed class CosmicBooksPostProcessorTests
{
    private MarkdownPipeline _mockMarkdownPipeline = null!;

    private CosmicBookPostProcessor _cosmicBookPostProcessor = null!;

    [SetUp]
    public void SetUp()
    {
        _mockMarkdownPipeline = new MarkdownPipelineBuilder().Build();
        _cosmicBookPostProcessor = new CosmicBookPostProcessor(_mockMarkdownPipeline);
    }

    [Test]
    public void Process_WithCosmicBooks_ReturnsProcessedCosmicBooks()
    {
        // arrange
        var cosmicBooks = new[]
        {
            new CosmicBook(
                Title: "Sample title",
                DatePublished: DateTime.MaxValue,
                Metadata: new CosmicBookMetadata(
                    Url: "https://example.com/sample-title",
                    Content: "Sample content",
                    Image: new CosmicImage(
                        Url: "https://example.com/sample-image"
                    ),
                    Author: "Sample author"
                )
            )
        };

        // act
        var result = _cosmicBookPostProcessor.Process(cosmicBooks);

        // assert
        result.Should().BeEquivalentTo(
            new[]
            {
                new CosmicBook(
                    Title: "Sample title",
                    DatePublished: DateTime.MaxValue,
                    Metadata: new CosmicBookMetadata(
                        Url: "https://example.com/sample-title",
                        Content: "<p>Sample content</p>\n",
                        Image: new CosmicImage(
                            Url: "https://example.com/sample-image"
                        ),
                        Author: "Sample author"
                    )
                )
            }
        );
    }
}
