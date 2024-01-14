using Coding.Blog.Library.Protos;

namespace Coding.Blog.Library.Adapters;

public sealed class PostsClientAdapter(Posts.PostsClient postsClient) : IProtoClientAdapter<Post>
{
    public async Task<IEnumerable<Post>> GetAsync()
    {
        var postsReply = await postsClient.GetPostsAsync(new PostsRequest()).ConfigureAwait(false);

        return postsReply.Posts;
    }
}
