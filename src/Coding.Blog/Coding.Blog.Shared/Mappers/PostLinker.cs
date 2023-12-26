using Coding.Blog.Shared.Domain;

namespace Coding.Blog.Shared.Mappers;

public sealed class PostLinker : IPostLinker
{
    public IEnumerable<Post> Link(IEnumerable<Post> posts)
    {
        var linkedPosts = new LinkedList<Post>(posts.OrderBy(post => post.DatePublished));

        var current = linkedPosts.First;

        while (current is not null)
        {
            current.Value = current.Value with
            {
                Previous = current.Previous?.Value,
                Next = current.Next?.Value
            };

            current = current.Next;
        }

        // Reverse the list here in order to start with the latest post first.
        return linkedPosts.Reverse();
    }
}
