using System.Diagnostics.CodeAnalysis;

namespace Coding.Blog.Engine.Configurations;

[ExcludeFromCodeCoverage]
public sealed record CosmicConfiguration : IKeyedConfiguration
{
    public string Key => "Cosmic";

    public string Endpoint { get; init; } = string.Empty;

    public string BucketSlug { get; init; } = string.Empty;

    public string ReadKey { get; init; } = string.Empty;
}