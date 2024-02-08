using Coding.Blog.Options;
using Microsoft.Extensions.Options;

namespace Coding.Blog.Services;

/// <summary>
///     Ensures that the application stops gracefully.
/// </summary>
/// <param name="hostApplicationLifetime"> The host application lifetime, leveraged to register the application stopping event. </param>
/// <param name="options"> The application lifetime options. </param>
internal sealed class ApplicationLifetimeService(
    IHostApplicationLifetime hostApplicationLifetime,
    IOptions<ApplicationLifetimeOptions> options
) : IHostedService
{
    private readonly TimeSpan _applicationStoppingGracePeriod = TimeSpan.FromSeconds(options.Value.ApplicationStoppingGracePeriodSeconds);

    public Task StartAsync(CancellationToken cancellationToken)
    {
        hostApplicationLifetime.ApplicationStopping.Register(() => Thread.Sleep(_applicationStoppingGracePeriod));

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
