using System.Text.Json.Serialization;

namespace Coding.Blog.DataTransfer;

internal sealed record CosmicPostMetadata(
    [property: JsonPropertyName("hero")] CosmicImage Image,
    [property: JsonPropertyName("tags")] string Tags,
    [property: JsonPropertyName("content")] string Content,
    [property: JsonPropertyName("description")] string Description
);
