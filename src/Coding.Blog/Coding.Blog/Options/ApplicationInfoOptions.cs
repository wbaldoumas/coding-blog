using System.ComponentModel.DataAnnotations;

namespace Coding.Blog.Options;

internal sealed record ApplicationInfoOptions
{
    public const string Key = "ApplicationInfo";

    [Required(ErrorMessage = "Value for {0} is required.")]
    public string Title { get; init; } = string.Empty;

    [Required(ErrorMessage = "Value for {0} is required.")]
    public string Description { get; init; } = string.Empty;
}
