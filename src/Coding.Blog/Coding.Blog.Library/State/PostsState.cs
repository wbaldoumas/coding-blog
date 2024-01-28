using Coding.Blog.Library.Domain;

namespace Coding.Blog.Library.State;

internal static class PostsState
{
    public static IEnumerable<Post> Posts { get; set; } = new List<Post>();
}
