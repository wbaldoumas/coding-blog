using System.ComponentModel.DataAnnotations;

namespace Coding.Blog.Options;

internal sealed record ResilienceOptions
{
    public const string Key = "Resilience";

    [Range(1, 1000, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
    public int MedianFirstRetryDelayMilliseconds { get; init; }

    [Range(1, 10, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
    public int RetryCount { get; init; }

    [Range(1, 86400000, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
    public int TimeToLiveMilliseconds { get; init; }
}
