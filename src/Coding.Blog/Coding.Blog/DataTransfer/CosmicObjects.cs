using System.Text.Json.Serialization;

namespace Coding.Blog.DataTransfer;

internal sealed record CosmicObjects<T>(
    [property: JsonPropertyName("objects")] IEnumerable<T> Objects
);
