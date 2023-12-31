using System.Text.Json.Serialization;

namespace Coding.Blog.Library.Records;

public sealed record CosmicProject(
    [property: JsonPropertyName("title")] string Title,
    [property: JsonPropertyName("metadata")] CosmicProjectMetadata Metadata
);
