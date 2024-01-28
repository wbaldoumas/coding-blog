using Coding.Blog.Clients;
using Coding.Blog.DataTransfer;
using Coding.Blog.Library.Protos;
using Coding.Blog.Library.Utilities;
using Grpc.Core;

namespace Coding.Blog.Services;

internal sealed class PostsService(ICosmicClient<CosmicPost> client, IMapper mapper) : Posts.PostsBase
{
    public override async Task<PostsReply> GetPosts(PostsRequest request, ServerCallContext context)
    {
        var cosmicPosts = await client.GetAsync().ConfigureAwait(false);
        var posts = mapper.Map<CosmicPost, Post>(cosmicPosts);

        return new PostsReply
        {
            Posts = { posts }
        };
    }
}
