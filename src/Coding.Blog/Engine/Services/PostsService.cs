using Coding.Blog.Engine.Clients;
using Coding.Blog.Engine.Mappers;
using Coding.Blog.Engine.Records;
using Grpc.Core;

namespace Coding.Blog.Engine.Services;

public sealed class PostsService : Posts.PostsBase
{
    private readonly ICosmicClient<CosmicPosts> _cosmicClient;
    private readonly IMapper<CosmicPost, Post> _mapper;
    private readonly IPostLinker _linker;

    public PostsService(ICosmicClient<CosmicPosts> cosmicClient, IMapper<CosmicPost, Post> mapper, IPostLinker linker)
    {
        _cosmicClient = cosmicClient;
        _mapper = mapper;
        _linker = linker;
    }

    public override async Task<PostsReply> GetPosts(PostsRequest request, ServerCallContext context)
    {
        var cosmicPosts = await _cosmicClient.GetAsync().ConfigureAwait(false);
        var posts = _mapper.Map(cosmicPosts.Posts);
        var linkedPosts = _linker.Link(posts);

        return new PostsReply
        {
            Posts = { linkedPosts }
        };
    }
}