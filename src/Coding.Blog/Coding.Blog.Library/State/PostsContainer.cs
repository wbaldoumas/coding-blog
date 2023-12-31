using Coding.Blog.Library.Domain;

namespace Coding.Blog.Library.State;

public static class PostsContainer
{
    public static IDictionary<string, Post> Posts { get; set; } = new Dictionary<string, Post>(StringComparer.Ordinal);
}
