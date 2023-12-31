using System.Text.Json.Serialization;

namespace Coding.Blog.Library.Records;

public sealed record CosmicBooks([property: JsonPropertyName("objects")] IEnumerable<CosmicBook> Books);
