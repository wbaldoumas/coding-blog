﻿using Coding.Blog.Shared.Records;

namespace Coding.Blog.Shared.Clients;

internal static class CosmicRequestRegistry
{
    public static readonly IDictionary<string, CosmicRequest> Requests = new Dictionary<string, CosmicRequest>
        (StringComparer.OrdinalIgnoreCase)
        {
            { typeof(CosmicBooks).FullName!, new CosmicRequest("books", "title,content,metadata,created_at") },
            { typeof(CosmicPosts).FullName!, new CosmicRequest("posts", "id,slug,title,metadata,created_at") }
        };
}