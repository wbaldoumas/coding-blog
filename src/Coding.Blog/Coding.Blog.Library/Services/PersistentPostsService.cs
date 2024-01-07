using Coding.Blog.Library.Domain;
using Coding.Blog.Library.State;
using Microsoft.AspNetCore.Components;

namespace Coding.Blog.Library.Services;

public class PersistentPostsService(IPostsService postsService) : IPersistentService<IDictionary<string, Post>>
{
    public async Task<IDictionary<string, Post>> GetAsync(PersistentComponentState applicationState, string stateKey)
    {
        if (applicationState.TryTakeFromJson<IDictionary<string, Post>>(stateKey, out var postsState))
        {
            PostsState.Posts = postsState ?? new Dictionary<string, Post>(StringComparer.Ordinal);
        }

        if (PostsState.Posts.Any())
        {
            return PostsState.Posts;
        }

        PostsState.Posts = await GetPostsAsync().ConfigureAwait(false);

        return PostsState.Posts;
    }

    private async Task<IDictionary<string, Post>> GetPostsAsync()
    {
        var posts = await postsService.GetAsync().ConfigureAwait(false);

        return posts.ToDictionary(post => post.Slug, post => post, StringComparer.Ordinal);
    }
}
