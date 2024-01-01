using Coding.Blog.Library.Domain;

namespace Coding.Blog.Library.State;

internal static class PostsState
{
    public static IDictionary<string, Post> Posts { get; set; } = new Dictionary<string, Post>(StringComparer.Ordinal);
}
