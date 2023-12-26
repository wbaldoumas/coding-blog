using Coding.Blog.Shared.Mappers;
using Coding.Blog.Shared.Protos;
using Post = Coding.Blog.Shared.Domain.Post;
using PostProto = Coding.Blog.Shared.Protos.Post;

namespace Coding.Blog.Shared.Services;

public class ClientPostsService(
    Posts.PostsClient postsClient,
    IMapper<PostProto, Post> postMapper,
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
