using Markdig;

namespace Coding.Blog.DataTransfer.PostProcessors;

internal sealed class CosmicPostsPostProcessor(MarkdownPipeline markdownPipeline) : ICosmicObjectPostProcessor<CosmicPost>
{
    public IEnumerable<CosmicPost> Process(IEnumerable<CosmicPost> cosmicObjects) =>
        from cosmicPost in cosmicObjects
        let cosmicPostMetadata = cosmicPost.Metadata with
        {
            Content = Markdig.Markdown.ToHtml(cosmicPost.Metadata.Content, markdownPipeline)
        }
        select cosmicPost with { Metadata = cosmicPostMetadata };
}
