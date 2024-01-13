using Coding.Blog.Library.Clients;
using Coding.Blog.Library.Mappers;
using Coding.Blog.Library.Protos;
using Coding.Blog.Library.Records;
using Grpc.Core;

namespace Coding.Blog.Library.Services;

public sealed class PostsService(
    ICosmicClient<CosmicPost> postsClient,
    IMapper<CosmicPost, Post> postMapper
) : Posts.PostsBase
{
    public override async Task<PostsReply> GetPosts(PostsRequest request, ServerCallContext context)
    {
        var cosmicPosts = await postsClient.GetAsync().ConfigureAwait(false);
        var posts = postMapper.Map(cosmicPosts);
        
        return new PostsReply
        {
            Posts = { posts }
        };
    }
}
