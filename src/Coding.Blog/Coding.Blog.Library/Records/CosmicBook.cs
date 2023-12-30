using System.Text.Json.Serialization;

namespace Coding.Blog.Library.Records;

public sealed record CosmicBook(
    [property: JsonPropertyName("title")] string Title,
    [property: JsonPropertyName("content")] string Content,
    [property: JsonPropertyName("created_at")] DateTime DatePublished,
    [property: JsonPropertyName("metadata")] CosmicBookMetadata Metadata
);
