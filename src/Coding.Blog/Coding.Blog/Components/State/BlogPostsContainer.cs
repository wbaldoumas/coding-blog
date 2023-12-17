using Coding.Blog.Shared.Domain;

namespace Coding.Blog.Components.State;

public static class BlogPostsContainer
{
    public static IDictionary<string, Post> Posts { get; set; } = new Dictionary<string, Post>();
}
