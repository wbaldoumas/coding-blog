using Markdig;

namespace Coding.Blog.Utilities;

internal sealed class StringSanitizer(MarkdownPipeline markdownPipeline) : IStringSanitizer
{
    // For now, just convert the markdown to plaintext. This method may be extended later.
    public string Sanitize(string content) => Markdig.Markdown.ToPlainText(content, markdownPipeline);
}
