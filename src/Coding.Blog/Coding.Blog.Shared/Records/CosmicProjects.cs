using System.Text.Json.Serialization;

namespace Coding.Blog.Shared.Records;

public sealed record CosmicProjects(
    [property: JsonPropertyName("objects")] IEnumerable<CosmicProject> Projects
);
