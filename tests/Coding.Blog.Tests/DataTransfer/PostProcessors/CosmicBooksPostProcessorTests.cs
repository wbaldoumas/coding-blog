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

    private CosmicBooksPostProcessor _cosmicBooksPostProcessor = null!;

    [SetUp]
    public void SetUp()
    {
        _mockMarkdownPipeline = new MarkdownPipelineBuilder().Build();
        _cosmicBooksPostProcessor = new CosmicBooksPostProcessor(_mockMarkdownPipeline);
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
        var result = _cosmicBooksPostProcessor.Process(cosmicBooks);

        // assert
        result.Should().BeEquivalentTo(
            [
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
            ]
        );
    }
}
