using System.Text.Json.Serialization;

namespace Coding.Blog.Library.Records;

public sealed record CosmicProjects(
    [property: JsonPropertyName("objects")] IEnumerable<CosmicProject> Projects
);
