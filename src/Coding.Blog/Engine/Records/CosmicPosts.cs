using Newtonsoft.Json;

namespace Coding.Blog.Engine.Records;

public record CosmicPosts(
    [property: JsonProperty("objects")] IEnumerable<CosmicPost> Posts
);

public record CosmicPost(
    [property: JsonProperty("id")] string Id,
    [property: JsonProperty("slug")] string Slug,
    [property: JsonProperty("title")] string Title,
    [property: JsonProperty("content")] string Content,
    [property: JsonProperty("created_at")] DateTime DatePublished,
    [property: JsonProperty("metadata")] CosmicPostMetadata Metadata
);

public record CosmicPostMetadata(
    [property: JsonProperty("hero")] CosmicPostHero Hero,
    [property: JsonProperty("tags")] string Tags
);

public record CosmicPostHero(
    [property: JsonProperty("url")] string Url,
    [property: JsonProperty("imgix_url")] string ImgixUrl
);
