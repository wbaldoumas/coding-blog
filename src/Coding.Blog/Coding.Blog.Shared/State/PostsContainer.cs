using Coding.Blog.Shared.Domain;

namespace Coding.Blog.Shared.State;

public static class PostsContainer
{
    public static IDictionary<string, Post> Posts { get; set; } = new Dictionary<string, Post>(StringComparer.Ordinal);
}
