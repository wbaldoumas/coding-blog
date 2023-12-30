using System.Text.Json.Serialization;

namespace Coding.Blog.Library.Records;

public sealed record CosmicBookMetadata(
    [property: JsonPropertyName("purchase_url")] string PurchaseUrl,
    [property: JsonPropertyName("cover")] CosmicBookCoverMetadata Cover
);
