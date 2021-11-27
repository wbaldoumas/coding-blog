using Newtonsoft.Json;

namespace Coding.Blog.Engine.Records;

public record CosmicBooks([property: JsonProperty("objects")] IEnumerable<CosmicBook> Books);

public record CosmicBook(
    [property: JsonProperty("title")] string Title,
    [property: JsonProperty("content")] string Content,
    [property: JsonProperty("published_at")] DateTime PublishedAt,
    [property: JsonProperty("metadata")] CosmicBookMetadata Metadata
);

public record CosmicBookMetadata(
    [property: JsonProperty("purchase_url")] string PurchaseUrl,
    [property: JsonProperty("cover")] CosmicBookCoverMetadata Cover
);

public record CosmicBookCoverMetadata([property: JsonProperty("url")] string Url);
