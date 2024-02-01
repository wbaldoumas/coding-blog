using Coding.Blog.Library.Protos;

namespace Coding.Blog.Client.Clients;

/// <summary>
///     Wraps the <see cref="Posts.PostsClient"/> to provide a more idiomatic, generic interface.
/// </summary>
/// <param name="postsClient">The <see cref="Posts.PostsClient"/> to wrap.</param>
internal sealed class PostsClient(Posts.PostsClient postsClient) : IProtoClient<Post>
{
    public async Task<IEnumerable<Post>> GetAsync()
    {
        var postsReply = await postsClient.GetPostsAsync(new PostsRequest()).ConfigureAwait(false);

        return postsReply.Posts;
    }
}
