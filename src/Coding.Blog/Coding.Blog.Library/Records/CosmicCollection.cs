using System.Text.Json.Serialization;

namespace Coding.Blog.Library.Records;

public sealed record CosmicCollection<T>(
    [property: JsonPropertyName("objects")] IEnumerable<T> Objects
);
