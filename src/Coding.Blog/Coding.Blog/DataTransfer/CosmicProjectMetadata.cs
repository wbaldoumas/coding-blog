using System.Text.Json.Serialization;

namespace Coding.Blog.DataTransfer;

internal sealed record CosmicProjectMetadata(
    [property: JsonPropertyName("description")] string Description,
    [property: JsonPropertyName("rank")] int Rank,
    [property: JsonPropertyName("github_url")] string GitHubUrl,
    [property: JsonPropertyName("hero")] CosmicImage Image,
    [property: JsonPropertyName("tags")] string Tags
);
