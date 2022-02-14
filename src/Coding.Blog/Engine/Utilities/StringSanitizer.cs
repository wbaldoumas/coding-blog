using Markdig;

namespace Coding.Blog.Engine.Utilities;

internal class StringSanitizer : IStringSanitizer
{
    private readonly MarkdownPipeline _markdownPipeline;

    public StringSanitizer(MarkdownPipeline markdownPipeline) => _markdownPipeline = markdownPipeline;

    // For now, just convert the markdown to plaintext. This method may be extended later.
    public string Sanitize(string content) => Markdown.ToPlainText(content, _markdownPipeline);
}
