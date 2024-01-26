using Microsoft.Extensions.Logging;

namespace Coding.Blog.Library.Extensions;

public static partial class LoggerExtensions
{
    [LoggerMessage(EventId = 2, Level = LogLevel.Information, Message = "Warming {DomainObject} cache...")]
    public static partial void LogCacheWarmingBegin(this ILogger logger, string domainObject);

    [LoggerMessage(EventId = 4, Level = LogLevel.Information, Message = "Finished warming {DomainObject} cache.")]
    public static partial void LogCacheWarmingEnd(this ILogger logger, string domainObject);

    [LoggerMessage(EventId = 1, Level = LogLevel.Critical, Message = "Failed to retrieve {DomainObject}s from Cosmic API: {Message}")]
    public static partial void LogCosmicApiError(this ILogger logger, string domainObject, string message);
}
