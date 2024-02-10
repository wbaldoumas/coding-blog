using System.Text.Json.Serialization;

namespace Coding.Blog.DataTransfer;

internal sealed record CosmicProjectMetadata(
    [property: JsonPropertyName("content")] string Content,
    [property: JsonPropertyName("rank")] int Rank,
    [property: JsonPropertyName("url")] string Url,
    [property: JsonPropertyName("hero")] CosmicImage Image,
    [property: JsonPropertyName("tags")] string Tags
);
