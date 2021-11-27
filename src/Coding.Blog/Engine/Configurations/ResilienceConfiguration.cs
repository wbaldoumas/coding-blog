using System.Diagnostics.CodeAnalysis;

namespace Coding.Blog.Engine.Configurations;

[ExcludeFromCodeCoverage]
public record ResilienceConfiguration : IKeyedConfiguration
{
    public string Key => "Resilience";

    public int MedianFirstRetryDelayMilliseconds { get; set; }

    public int RetryCount { get; set; }

    public int TimeToLiveMilliseconds { get; set; }
}