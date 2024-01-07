using Coding.Blog.Library.Mappers;
using Coding.Blog.Library.Protos;
using Coding.Blog.Library.Utilities;
using Post = Coding.Blog.Library.Domain.Post;
using ProtoPost = Coding.Blog.Library.Protos.Post;

namespace Coding.Blog.Library.Services;

public sealed class ClientPostsService(
    Posts.PostsClient postsClient,
    IMapper<ProtoPost, Post> postMapper,
    IPostLinker postLinker
) : IPostsService
{
    public async Task<IEnumerable<Post>> GetAsync()
    {
        var postsReply = await postsClient.GetPostsAsync(new PostsRequest()).ConfigureAwait(false);
        var posts = postMapper.Map(postsReply.Posts);

        return postLinker.Link(posts);
    }
}
