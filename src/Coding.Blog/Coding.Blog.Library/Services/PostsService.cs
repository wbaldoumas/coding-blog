using Coding.Blog.Library.Clients;
using Coding.Blog.Library.Domain;
using Coding.Blog.Library.Mappers;
using Coding.Blog.Library.Records;
using Coding.Blog.Library.Utilities;

namespace Coding.Blog.Library.Services;

public sealed class PostsService(
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
