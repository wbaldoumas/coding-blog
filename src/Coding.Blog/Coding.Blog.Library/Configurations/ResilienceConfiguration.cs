using System.Diagnostics.CodeAnalysis;

namespace Coding.Blog.Library.Configurations;

[ExcludeFromCodeCoverage]
public sealed record ResilienceConfiguration
{
    public int MedianFirstRetryDelayMilliseconds { get; init; }

    public int RetryCount { get; init; }

    public int TimeToLiveMilliseconds { get; init; }
}
