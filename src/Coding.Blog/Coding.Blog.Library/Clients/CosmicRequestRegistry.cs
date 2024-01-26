using Coding.Blog.Library.DataTransfer;

namespace Coding.Blog.Library.Clients;

internal static class CosmicRequestRegistry
{
    public static readonly IDictionary<string, CosmicRequest> Requests = new Dictionary<string, CosmicRequest>(StringComparer.OrdinalIgnoreCase)
    {
        { typeof(CosmicBook).FullName!, new CosmicRequest("books", "title,metadata,created_at") },
        { typeof(CosmicPost).FullName!, new CosmicRequest("posts", "id,slug,title,metadata,created_at") },
        { typeof(CosmicProject).FullName!, new CosmicRequest("projects", "title,metadata") }
    };
}
