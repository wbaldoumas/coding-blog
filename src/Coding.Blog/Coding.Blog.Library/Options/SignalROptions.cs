using System.ComponentModel.DataAnnotations;

namespace Coding.Blog.Library.Options;

public sealed record SignalROptions
{
    public const string Key = "SignalR";

    [Range(1024, 2048000, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
    public int MaximumReceiveMessageSize { get; init; }

    [Range(1, 600, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
    public int KeepAliveIntervalSeconds { get; init; }

    public TimeSpan KeepAliveInterval => TimeSpan.FromSeconds(KeepAliveIntervalSeconds);

    public bool EnableDetailedErrors { get; init; }

    [Range(1, 60, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
    public int HandshakeTimeoutSeconds { get; init; }

    public TimeSpan HandshakeTimeout => TimeSpan.FromSeconds(HandshakeTimeoutSeconds);

    [Range(1, 60 * 24, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
    public int ClientTimeoutIntervalMinutes { get; init; }

    public TimeSpan ClientTimeoutInterval => TimeSpan.FromMinutes(ClientTimeoutIntervalMinutes);

    [Range(1, 100, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
    public int StreamBufferCapacity { get; init; }

    [Range(1024, 2048000, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
    public int StatefulReconnectBufferSize { get; init; }
}
