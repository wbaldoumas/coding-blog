using System.Text.Json.Serialization;

namespace Coding.Blog.Shared.Records;

public sealed record CosmicPosts(
    [property: JsonPropertyName("objects")] IEnumerable<CosmicPost> Posts
);
