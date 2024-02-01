using Markdig;

namespace Coding.Blog.Utilities;

/// <summary>
///     A utility for sanitizing strings.
/// </summary>
/// <param name="markdownPipeline">A <see cref="MarkdownPipeline"/> for converting markdown to plaintext.</param>
internal sealed class StringSanitizer(MarkdownPipeline markdownPipeline) : IStringSanitizer
{
    // For now, just convert the markdown to plaintext. This method may be extended later.
    public string Sanitize(string content) => Markdig.Markdown.ToPlainText(content, markdownPipeline);
}
