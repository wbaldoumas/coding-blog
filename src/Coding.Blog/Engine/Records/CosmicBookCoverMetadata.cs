using Newtonsoft.Json;

namespace Coding.Blog.Engine.Records;

public sealed record CosmicBookCoverMetadata([property: JsonProperty("url")] string Url);