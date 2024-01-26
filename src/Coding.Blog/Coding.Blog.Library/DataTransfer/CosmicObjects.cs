using System.Text.Json.Serialization;

namespace Coding.Blog.Library.DataTransfer;

public sealed record CosmicObjects<T>(
    [property: JsonPropertyName("objects")] IEnumerable<T> Objects
);
