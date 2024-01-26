using System.Text.Json.Serialization;

namespace Coding.Blog.Library.DataTransfer;

public sealed record CosmicPostMetadata(
    [property: JsonPropertyName("hero")] CosmicImage Image,
    [property: JsonPropertyName("tags")] string Tags,
    [property: JsonPropertyName("markdown")] string Markdown,
    [property: JsonPropertyName("description")] string Description
);
