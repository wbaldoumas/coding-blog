namespace Coding.Blog.Engine.Mappers;

public sealed class PostLinker : IPostLinker
{
    public IReadOnlyCollection<Post> Link(IEnumerable<Post> posts)
    {
        var sortedPosts = posts.OrderByDescending(post => post.DatePublished).ToList();

        var linkedPosts = new List<Post>();

        foreach (var (post, index) in sortedPosts.Select((post, index) => (post, index)))
        {
            var linkedPost = new Post(post);

            if (index > 0)
            {
                linkedPost.Next = sortedPosts[index - 1];
            }

            if (index < sortedPosts.Count - 1)
            {
                linkedPost.Previous = sortedPosts[index + 1];
            }

            linkedPosts.Add(linkedPost);
        }

        return linkedPosts;
    }
}