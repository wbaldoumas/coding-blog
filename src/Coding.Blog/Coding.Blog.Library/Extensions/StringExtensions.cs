using Markdig;
using Microsoft.AspNetCore.Components;

namespace Coding.Blog.Library.Extensions;

public static class StringExtensions
{
    public static MarkupString ToMarkupString(this string source, MarkdownPipeline markdownPipeline) => new(Markdown.ToHtml(source, markdownPipeline));

    public static IEnumerable<string> ToDisplayTags(this string source) => source.Trim().Length > 0
        ? source.Split(",").Select(tag => tag.Trim())
        : Array.Empty<string>();
}
