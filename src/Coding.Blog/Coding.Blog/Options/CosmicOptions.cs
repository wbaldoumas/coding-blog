using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Coding.Blog.Options;

[ExcludeFromCodeCoverage]
internal sealed record CosmicOptions
{
    public const string Key = "Cosmic";

    [Url(ErrorMessage = "Value for {0} must be a valid URL.")]
    public string Endpoint { get; init; } = string.Empty;

    [Required(ErrorMessage = "Value for {0} is required.")]
    public string BucketSlug { get; init; } = string.Empty;

    [Required(ErrorMessage = "Value for {0} is required.")]
    public string ReadKey { get; init; } = string.Empty;
}
