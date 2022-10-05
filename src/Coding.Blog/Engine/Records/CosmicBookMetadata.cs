using Newtonsoft.Json;

namespace Coding.Blog.Engine.Records;

public sealed record CosmicBookMetadata(
    [property: JsonProperty("purchase_url")] string PurchaseUrl,
    [property: JsonProperty("cover")] CosmicBookCoverMetadata Cover
);