using Newtonsoft.Json;

namespace Coding.Blog.Engine.Records;

public sealed record CosmicPosts(
    [property: JsonProperty("objects")] IEnumerable<CosmicPost> Posts
);