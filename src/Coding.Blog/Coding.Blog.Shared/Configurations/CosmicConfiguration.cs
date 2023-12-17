using System.Diagnostics.CodeAnalysis;

namespace Coding.Blog.Shared.Configurations;

[ExcludeFromCodeCoverage]
public sealed record CosmicConfiguration
{
    public string Endpoint { get; init; } = string.Empty;

    public string BucketSlug { get; init; } = string.Empty;

    public string ReadKey { get; init; } = string.Empty;
}
