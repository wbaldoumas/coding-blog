﻿using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Options;

namespace Coding.Blog.Options;

[ExcludeFromCodeCoverage]
internal sealed record CacheWarmingJobOptions
{
    public const string Key = "Quartz";

    [ValidateObjectMembers]
    public QuartzJobOptions BooksWarmingJob { get; init; } = null!;

    [ValidateObjectMembers]
    public QuartzJobOptions PostsWarmingJob { get; init; } = null!;

    [ValidateObjectMembers]
    public QuartzJobOptions ProjectsWarmingJob { get; init; } = null!;
}
