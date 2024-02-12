using Markdig;

namespace Coding.Blog.DataTransfer.PostProcessors;

internal sealed class CosmicProjectsPostProcessor(MarkdownPipeline markdownPipeline) : ICosmicObjectPostProcessor<CosmicProject>
{
    public IEnumerable<CosmicProject> Process(IEnumerable<CosmicProject> cosmicObjects) =>
        from cosmicProject in cosmicObjects
        let cosmicProjectMetadata = cosmicProject.Metadata with
        {
            Content = Markdig.Markdown.ToHtml(cosmicProject.Metadata.Content, markdownPipeline)
        }
        select cosmicProject with { Metadata = cosmicProjectMetadata };
}
