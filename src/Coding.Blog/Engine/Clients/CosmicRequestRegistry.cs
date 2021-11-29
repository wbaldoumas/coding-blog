using Coding.Blog.Engine.Records;

namespace Coding.Blog.Engine.Clients;

internal record CosmicRequest(string Type, string Props);

internal static class CosmicRequestRegistry
{
    public static readonly IDictionary<string, CosmicRequest> Requests = new Dictionary<string, CosmicRequest>
    {
        { typeof(CosmicBooks).FullName!, new CosmicRequest("books", "title,content,metadata,created_at") },
        { typeof(CosmicPosts).FullName!, new CosmicRequest("posts", "id,slug,title,content,metadata,created_at") },
    };
}
