using Coding.Blog.Shared.Clients;
using Coding.Blog.Shared.Domain;
using Coding.Blog.Shared.Mappers;
using Coding.Blog.Shared.Records;

namespace Coding.Blog.Shared.Services;

internal sealed class PostsService(
    ICosmicClient<CosmicPosts> postsClient,
    IMapper<CosmicPost, Post> postsMapper,
    IPostLinker linker) : IPostsService
{
    public async Task<IEnumerable<Post>> GetAsync()
    {
        var cosmicPosts = await postsClient.GetAsync().ConfigureAwait(false);
        var posts = postsMapper.Map(cosmicPosts.Posts);

        return linker.Link(posts);
    }
}
