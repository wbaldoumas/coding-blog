using Coding.Blog.Shared.Clients;
using Coding.Blog.Shared.Domain;
using Coding.Blog.Shared.Mappers;
using Coding.Blog.Shared.Records;

namespace Coding.Blog.Shared.Services;

internal sealed class PostsService(
    ICosmicClient<CosmicPosts> postsClient,
    IMapper<CosmicPost, Post> postMapper,
    IPostLinker postLinker
) : IPostsService
{
    public async Task<IEnumerable<Post>> GetAsync()
    {
        var cosmicPosts = await postsClient.GetAsync().ConfigureAwait(false);
        var posts = postMapper.Map(cosmicPosts.Posts);

        return postLinker.Link(posts);
    }
}
