using System.Text.Json.Serialization;

namespace Coding.Blog.Library.DataTransfer;

public sealed record CosmicBook(
    [property: JsonPropertyName("title")] string Title,
    [property: JsonPropertyName("content")] string Content,
    [property: JsonPropertyName("created_at")] DateTime DatePublished,
    [property: JsonPropertyName("metadata")] CosmicBookMetadata Metadata
);
