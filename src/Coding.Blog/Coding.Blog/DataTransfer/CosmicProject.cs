using System.Text.Json.Serialization;

namespace Coding.Blog.DataTransfer;

internal sealed record CosmicProject(
    [property: JsonPropertyName("title")] string Title,
    [property: JsonPropertyName("metadata")] CosmicProjectMetadata Metadata
);
