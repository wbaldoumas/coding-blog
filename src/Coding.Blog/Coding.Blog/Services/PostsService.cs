using Coding.Blog.Clients;
using Coding.Blog.DataTransfer;
using Coding.Blog.Library.Protos;
using Coding.Blog.Library.Utilities;
using Grpc.Core;

namespace Coding.Blog.Services;

/// <summary>
///     A gRPC service for the posts endpoint.
/// </summary>
/// <param name="client">The <see cref="ICosmicClient{T}"/> to retrieve posts from.</param>
/// <param name="mapper">A <see cref="IMapper"/> for mapping objects to their protobuf representations.</param>
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
