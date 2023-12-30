using Coding.Blog.Library.Clients;
using Coding.Blog.Library.Mappers;
using Coding.Blog.Library.Records;
using Coding.Blog.Library.Protos;
using Grpc.Core;

namespace Coding.Blog.Library.Services;

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
