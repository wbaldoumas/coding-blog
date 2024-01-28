using System.Text.Json.Serialization;

namespace Coding.Blog.DataTransfer;

internal sealed record CosmicImage(
    [property: JsonPropertyName("url")] string Url,
    [property: JsonPropertyName("imgix_url")] string ImgixUrl
);
