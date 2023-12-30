using System.Text.Json.Serialization;

namespace Coding.Blog.Library.Records;

public sealed record CosmicPosts(
    [property: JsonPropertyName("objects")] IEnumerable<CosmicPost> Posts
);
