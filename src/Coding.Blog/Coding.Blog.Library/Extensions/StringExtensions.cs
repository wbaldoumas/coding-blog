using Markdig;
using Microsoft.AspNetCore.Components;

namespace Coding.Blog.Library.Extensions;

public static class StringExtensions
{
    public static MarkupString ToMarkupString(this string source, MarkdownPipeline markdownPipeline) => new(Markdown.ToHtml(source, markdownPipeline));
}
