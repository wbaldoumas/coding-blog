using Coding.Blog.Library.Domain;

namespace Coding.Blog.Library.State;

public static class PostsState
{
    public const string Key = "Posts";

    public static IEnumerable<Post> Posts { get; set; } = new List<Post>();
}
