using System.Text.Json.Serialization;

namespace Coding.Blog.Library.Records;

public sealed record CosmicPostMetadata(
    [property: JsonPropertyName("hero")] CosmicHero Hero,
    [property: JsonPropertyName("tags")] string Tags,
    [property: JsonPropertyName("markdown")] string Markdown
);
