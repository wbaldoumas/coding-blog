using Newtonsoft.Json;

namespace Coding.Blog.Engine.Records;

public sealed record CosmicBooks([property: JsonProperty("objects")] IEnumerable<CosmicBook> Books);