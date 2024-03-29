﻿using Coding.Blog.Library.Domain;

namespace Coding.Blog.Library.Utilities;

/// <summary>
///     Links the given posts to one another sequentially by date, hydrating the <see cref="Post.Next"/> and <see cref="Post.Previous"/> properties.
/// </summary>
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
