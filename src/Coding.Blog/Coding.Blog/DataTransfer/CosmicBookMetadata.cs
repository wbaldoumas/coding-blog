using System.Text.Json.Serialization;

namespace Coding.Blog.DataTransfer;

internal sealed record CosmicBookMetadata(
    [property: JsonPropertyName("purchase_url")] string PurchaseUrl,
    [property: JsonPropertyName("cover")] CosmicImage Image,
    [property: JsonPropertyName("content")] string Content,
    [property: JsonPropertyName("author")] string Author
);
