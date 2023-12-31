using System.Text.Json.Serialization;

namespace Coding.Blog.Library.Records;

public sealed record CosmicHero(
    [property: JsonPropertyName("url")] string Url,
    [property: JsonPropertyName("imgix_url")] string ImgixUrl
);
