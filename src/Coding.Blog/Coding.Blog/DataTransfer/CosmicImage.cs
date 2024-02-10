using System.Text.Json.Serialization;

namespace Coding.Blog.DataTransfer;

internal sealed record CosmicImage(
    [property: JsonPropertyName("imgix_url")] string Url
);
