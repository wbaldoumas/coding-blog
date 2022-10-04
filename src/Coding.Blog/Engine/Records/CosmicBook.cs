using Newtonsoft.Json;

namespace Coding.Blog.Engine.Records;

public sealed record CosmicBook(
    [property: JsonProperty("title")] string Title,
    [property: JsonProperty("content")] string Content,
    [property: JsonProperty("created_at")] DateTime DatePublished,
    [property: JsonProperty("metadata")] CosmicBookMetadata Metadata
);