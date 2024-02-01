using Markdig;
using Microsoft.AspNetCore.Components;

namespace Coding.Blog.Library.Extensions;

public static class StringExtensions
{
    /// <summary>
    ///     Converts the given string to a <see cref="MarkupString"/>.
    /// </summary>
    /// <param name="source">The source string to convert.</param>
    /// <param name="markdownPipeline">A <see cref="MarkdownPipeline"/> to use for the conversion.</param>
    /// <returns>The converted <see cref="MarkupString"/>.</returns>
    public static MarkupString ToMarkupString(this string source, MarkdownPipeline markdownPipeline) => new(Markdown.ToHtml(source, markdownPipeline));

    /// <summary>
    ///     Parses the given string into a collection of display tags.
    /// </summary>
    /// <param name="source">The source string to parse.</param>
    /// <returns>The collection of display tags.</returns>
    public static IEnumerable<string> ToDisplayTags(this string source) => source.Trim().Length > 0
        ? source.Split(",").Select(tag => tag.Trim())
        : Array.Empty<string>();
}
