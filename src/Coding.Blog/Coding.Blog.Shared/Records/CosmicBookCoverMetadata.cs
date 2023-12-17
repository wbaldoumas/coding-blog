using System.Text.Json.Serialization;

namespace Coding.Blog.Shared.Records;

public sealed record CosmicBookCoverMetadata([property: JsonPropertyName("url")] string Url);
