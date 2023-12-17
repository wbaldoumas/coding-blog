using System.Text.Json.Serialization;

namespace Coding.Blog.Shared.Records;

public sealed record CosmicPostHero(
    [property: JsonPropertyName("url")] string Url,
    [property: JsonPropertyName("imgix_url")] string ImgixUrl
);
