using System.Text.Json.Serialization;

namespace Coding.Blog.Library.Records;

public sealed record CosmicBookCoverMetadata([property: JsonPropertyName("url")] string Url);
