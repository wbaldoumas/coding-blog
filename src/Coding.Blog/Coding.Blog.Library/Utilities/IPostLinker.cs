using Coding.Blog.Library.Domain;

namespace Coding.Blog.Library.Utilities;

/// <summary>
///     Links the given posts to one another, hydrating the <see cref="Post.Next"/> and <see cref="Post.Previous"/> properties.
/// </summary>
public interface IPostLinker
{
    IEnumerable<Post> Link(IEnumerable<Post> posts);
}
