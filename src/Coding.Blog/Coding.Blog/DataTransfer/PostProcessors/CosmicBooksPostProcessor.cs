using Markdig;

namespace Coding.Blog.DataTransfer.PostProcessors;

internal sealed class CosmicBooksPostProcessor(MarkdownPipeline markdownPipeline) : ICosmicObjectPostProcessor<CosmicBook>
{
    public IEnumerable<CosmicBook> Process(IEnumerable<CosmicBook> cosmicObjects) =>
        from cosmicBook in cosmicObjects
        let cosmicBookMetadata = cosmicBook.Metadata with
        {
            Content = Markdig.Markdown.ToHtml(cosmicBook.Metadata.Content, markdownPipeline)
        }
        select cosmicBook with { Metadata = cosmicBookMetadata };
}
