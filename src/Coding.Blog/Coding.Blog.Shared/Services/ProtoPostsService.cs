using Coding.Blog.Shared.Clients;
using Coding.Blog.Shared.Mappers;
using Coding.Blog.Shared.Protos;
using Coding.Blog.Shared.Records;
using Grpc.Core;

namespace Coding.Blog.Shared.Services;

public sealed class ProtoPostsService(
    ICosmicClient<CosmicPosts> postsClient,
    IMapper<CosmicPost, Post> postMapper
) : Posts.PostsBase
{
    public override async Task<PostsReply> GetPosts(PostsRequest request, ServerCallContext context)
    {
        var cosmicPosts = await postsClient.GetAsync().ConfigureAwait(false);
        var posts = postMapper.Map(cosmicPosts.Posts);
        
        return new PostsReply
        {
            Posts = { posts }
        };
    }
}
