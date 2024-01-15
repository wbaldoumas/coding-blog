using System.Text.Json.Serialization;

namespace Coding.Blog.Library.DataTransfer;

public sealed record CosmicImage(
    [property: JsonPropertyName("url")] string Url,
    [property: JsonPropertyName("imgix_url")] string ImgixUrl
);
