using System.Diagnostics.CodeAnalysis;
using Coding.Blog.Configurations;

namespace Coding.Blog.HostedServices;

[ExcludeFromCodeCoverage]
internal sealed class ApplicationLifetimeService(
    IHostApplicationLifetime hostApplicationLifetime,
    ApplicationLifetimeConfiguration configuration
) : IHostedService
{
    private readonly TimeSpan _applicationStoppingGracePeriod = TimeSpan.FromSeconds(configuration.ApplicationStoppingGracePeriodSeconds);

    public Task StartAsync(CancellationToken cancellationToken)
    {
        hostApplicationLifetime.ApplicationStopping.Register(() => Thread.Sleep(_applicationStoppingGracePeriod));

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
