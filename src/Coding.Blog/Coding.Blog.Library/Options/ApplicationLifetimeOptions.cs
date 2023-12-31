using System.ComponentModel.DataAnnotations;

namespace Coding.Blog.Library.Options;

/// <summary>
///     Controls how long the application should stay alive after receiving a SIGTERM
///     signal. This prevents in-flight traffic from being dropped during deployments or
///     application restarts.
/// </summary>
public sealed class ApplicationLifetimeOptions
{
    public const string Key = "ApplicationLifetime";

    [Range(1, 60, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
    public int ApplicationStoppingGracePeriodSeconds { get; init; }
}
