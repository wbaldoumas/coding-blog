using System.Diagnostics.CodeAnalysis;

namespace Coding.Blog.Engine.Configurations;

[ExcludeFromCodeCoverage]
public sealed record GrpcResilienceConfiguration : IKeyedConfiguration
{
    public string Key => "GrpcResilience";

    public int MaxAttempts { get; set; }

    public int InitialBackoffMilliseconds { get; set; }

    public int MaxBackoffMilliseconds { get; set; }

    public double BackoffMultiplier { get; set; }
}