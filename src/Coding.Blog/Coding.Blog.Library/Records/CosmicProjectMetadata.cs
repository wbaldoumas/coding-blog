using System.Text.Json.Serialization;

namespace Coding.Blog.Library.Records;

public sealed record CosmicProjectMetadata(
    [property: JsonPropertyName("description")] string Description,
    [property: JsonPropertyName("rank")] int Rank,
    [property: JsonPropertyName("github_url")] string GitHubUrl,
    [property: JsonPropertyName("hero")] CosmicHero Hero,
    [property: JsonPropertyName("tags")] string Tags
);
