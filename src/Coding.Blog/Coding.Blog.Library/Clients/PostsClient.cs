using Coding.Blog.Library.Protos;

namespace Coding.Blog.Library.Clients;

public sealed class PostsClient(Posts.PostsClient postsClient) : IProtoClient<Post>
{
    public async Task<IEnumerable<Post>> GetAsync()
    {
        var postsReply = await postsClient.GetPostsAsync(new PostsRequest()).ConfigureAwait(false);

        return postsReply.Posts;
    }
}
