using Markdig;

namespace Coding.Blog.Library.Utilities;

public sealed class StringSanitizer(MarkdownPipeline markdownPipeline) : IStringSanitizer
{
    // For now, just convert the markdown to plaintext. This method may be extended later.
    public string Sanitize(string content) => Markdown.ToPlainText(content, markdownPipeline);
}
