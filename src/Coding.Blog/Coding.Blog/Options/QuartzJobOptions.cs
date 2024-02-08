using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Coding.Blog.Options;

[ExcludeFromCodeCoverage]
internal sealed record QuartzJobOptions
{
    [Required]
    public string JobKey { get; init; } = string.Empty;

    [Range(1, 86400, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
    public int IntervalSeconds { get; init; }
}
