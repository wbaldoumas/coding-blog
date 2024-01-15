using System.Text.Json.Serialization;

namespace Coding.Blog.Library.DataTransfer;

public sealed record CosmicProject(
    [property: JsonPropertyName("title")] string Title,
    [property: JsonPropertyName("metadata")] CosmicProjectMetadata Metadata
);
