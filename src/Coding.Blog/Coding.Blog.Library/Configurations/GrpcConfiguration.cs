using System.Diagnostics.CodeAnalysis;

namespace Coding.Blog.Library.Configurations;

[ExcludeFromCodeCoverage]
public sealed record GrpcConfiguration
{
    public int MaxAttempts { get; init; }

    public int InitialBackoffMilliseconds { get; init; }

    public TimeSpan InitialBackoff => TimeSpan.FromMilliseconds(InitialBackoffMilliseconds);

    public int MaxBackoffMilliseconds { get; init; }

    public TimeSpan MaxBackoff => TimeSpan.FromMilliseconds(MaxBackoffMilliseconds);

    public int BackoffMultiplier { get; init; }

}
