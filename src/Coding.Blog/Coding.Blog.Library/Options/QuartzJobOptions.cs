using System.ComponentModel.DataAnnotations;

namespace Coding.Blog.Library.Options;

public sealed record QuartzJobOptions
{
    [Range(60, 86400, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
    public int IntervalSeconds { get; init; }
}
