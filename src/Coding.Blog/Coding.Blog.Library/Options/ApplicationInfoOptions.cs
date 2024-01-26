using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Coding.Blog.Library.Options;

[ExcludeFromCodeCoverage]
public sealed record ApplicationInfoOptions
{
    public const string Key = "ApplicationInfo";

    [Required(ErrorMessage = "Value for {0} is required.")]
    public string Title { get; init; } = string.Empty;

    [Required(ErrorMessage = "Value for {0} is required.")]
    public string Description { get; init; } = string.Empty;
}
