namespace Coding.Blog.Engine.Mappers;

/// <summary>
///     Sorts posts by creation date, then links posts to their "previous" and "next" posts.
/// </summary>
public interface IPostLinker
{
    /// <summary>
    ///     Sort posts by creation date, then link posts to their "previous" and "next" post.
    /// </summary>
    /// <param name="posts">The posts to link</param>
    /// <returns>An <see cref="IReadOnlyCollection{T}"/> of <see cref="Post"/></returns>
    IReadOnlyCollection<Post> Link(IEnumerable<Post> posts);
}