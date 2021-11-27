using System.Diagnostics.CodeAnalysis;

namespace Coding.Blog.Engine.Configurations;

[ExcludeFromCodeCoverage]
public class ResilienceConfiguration : IKeyedConfiguration
{
    public string Key => "Resilience";

    public const int DefaultMedianFirstRetryDelayMilliseconds = 50;

    public const int DefaultRetryCount = 5;

    public const int DefaultTimeToLiveMilliseconds = 86400000;

    public int MedianFirstRetryDelayMilliseconds { get; set; }

    public int RetryCount { get; set; }

    public int TimeToLiveMilliseconds { get; set; }
}