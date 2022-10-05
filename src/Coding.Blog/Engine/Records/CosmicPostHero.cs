using Newtonsoft.Json;

namespace Coding.Blog.Engine.Records;

public sealed record CosmicPostHero(
    [property: JsonProperty("url")] string Url,
    [property: JsonProperty("imgix_url")] string ImgixUrl
);