using Newtonsoft.Json;

namespace Coding.Blog.Engine.Records;

public sealed record CosmicPostMetadata(
    [property: JsonProperty("hero")] CosmicPostHero Hero,
    [property: JsonProperty("tags")] string Tags,
    [property: JsonProperty("markdown")] string Markdown
);