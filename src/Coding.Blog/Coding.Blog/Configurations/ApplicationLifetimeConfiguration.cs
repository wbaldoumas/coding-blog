﻿namespace Coding.Blog.Configurations;

/// <summary>
///     Controls how long the application should stay alive after receiving a SIGTERM
///     signal. This prevents in-flight traffic from being dropped during deployments or
///     application restarts.
/// </summary>
internal sealed class ApplicationLifetimeConfiguration
{
    public int ApplicationStoppingGracePeriodSeconds { get; init; }
}