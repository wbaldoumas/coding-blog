using Coding.Blog.Library.Domain;

namespace Coding.Blog.Library.State;

internal static class PostsState
{
    public const string Key = "Posts";

    public static IList<Post> Posts { get; set; } = new List<Post>();
}
