using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Coding.Blog.Client.Options;

[ExcludeFromCodeCoverage]
internal sealed record GrpcOptions
{
    public const string Key = "Grpc";

    [Range(1, 10, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
    public int MaxAttempts { get; init; }

    [Range(1, 1000, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
    public int InitialBackoffMilliseconds { get; init; }

    public TimeSpan InitialBackoff => TimeSpan.FromMilliseconds(InitialBackoffMilliseconds);

    [Range(1, 1000, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
    public int MaxBackoffMilliseconds { get; init; }

    public TimeSpan MaxBackoff => TimeSpan.FromMilliseconds(MaxBackoffMilliseconds);

    [Range(1, 10, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
    public int BackoffMultiplier { get; init; }

}
