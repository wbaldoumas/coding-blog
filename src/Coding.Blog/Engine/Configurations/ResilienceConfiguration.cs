﻿using System.Diagnostics.CodeAnalysis;

namespace Coding.Blog.Engine.Configurations;

[ExcludeFromCodeCoverage]
public sealed record ResilienceConfiguration
{
    public static string Key => "Resilience";

    public int MedianFirstRetryDelayMilliseconds { get; init; }

    public int RetryCount { get; init; }

    public int TimeToLiveMilliseconds { get; init; }
}