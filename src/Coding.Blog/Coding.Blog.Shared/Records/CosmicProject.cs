using System.Text.Json.Serialization;

namespace Coding.Blog.Shared.Records;

public record CosmicProject(
    [property: JsonPropertyName("title")] string Title,
    [property: JsonPropertyName("metadata")] CosmicProjectMetadata Metadata
);
