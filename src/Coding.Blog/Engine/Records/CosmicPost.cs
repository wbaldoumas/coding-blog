using Newtonsoft.Json;

namespace Coding.Blog.Engine.Records;

public sealed record CosmicPost(
    [property: JsonProperty("id")] string Id,
    [property: JsonProperty("slug")] string Slug,
    [property: JsonProperty("title")] string Title,
    [property: JsonProperty("created_at")] DateTime DatePublished,
    [property: JsonProperty("metadata")] CosmicPostMetadata Metadata
);