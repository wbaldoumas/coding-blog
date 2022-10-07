using System.Diagnostics.CodeAnalysis;

namespace Coding.Blog.Engine.Configurations;

[ExcludeFromCodeCoverage]
public sealed record GrpcResilienceConfiguration
{
    public static string Key => "GrpcResilience";

    public int MaxAttempts { get; init; }

    public int InitialBackoffMilliseconds { get; init; }

    public int MaxBackoffMilliseconds { get; init; }

    public double BackoffMultiplier { get; init; }
}