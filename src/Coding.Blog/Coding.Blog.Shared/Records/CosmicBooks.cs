using System.Text.Json.Serialization;

namespace Coding.Blog.Shared.Records;

public sealed record CosmicBooks([property: JsonPropertyName("objects")] IEnumerable<CosmicBook> Books);
