using System.Diagnostics.CodeAnalysis;

namespace Coding.Blog.Engine.Configurations;

[ExcludeFromCodeCoverage]
public class CosmicConfiguration : IKeyedConfiguration
{
    public string Key => "Cosmic";

    public string Endpoint { get; set; } = string.Empty;

    public string BucketSlug { get; set; } = string.Empty;

    public string ReadKey { get; set; } = string.Empty;
}