using System.Diagnostics.CodeAnalysis;
using Coding.Blog.Library.Configurations;
using Microsoft.Extensions.Hosting;

namespace Coding.Blog.Library.Services;

[ExcludeFromCodeCoverage]
public sealed class ApplicationLifetimeService(
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
