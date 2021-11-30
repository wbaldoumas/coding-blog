using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Logging;
using Polly;

namespace Coding.Blog.Engine.Clients;

[ExcludeFromCodeCoverage]
public class ResilientPostsClient : IResilientClient<Post>
{
    private readonly Posts.PostsClient _postsClient;
    private readonly ILogger<Post> _logger;
    private readonly IAsyncPolicy<IEnumerable<Post>> _resiliencePolicy;

    public ResilientPostsClient(
        Posts.PostsClient postsClient,
        ILogger<Post> logger,
        IAsyncPolicy<IEnumerable<Post>> resiliencePolicy)
    {
        _postsClient = postsClient;
        _logger = logger;
        _resiliencePolicy = resiliencePolicy;
    }

    public async Task<IEnumerable<Post>> GetAsync()
    {
        try
        {
            return await _resiliencePolicy.ExecuteAsync(async _ =>
                {
                    var booksReply = await _postsClient.GetPostsAsync(new PostsRequest());

                    return booksReply.Posts;
                },
                new Context("GetPosts")
            );
        }
        catch (Exception exception)
        {
            _logger.LogError($"Unable to retrieve posts from backend service: {exception.Message}!");

            throw;
        }
    }
}