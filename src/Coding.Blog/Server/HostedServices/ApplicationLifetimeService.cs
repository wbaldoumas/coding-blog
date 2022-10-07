using System.Diagnostics.CodeAnalysis;
using Coding.Blog.Server.Configurations;
using Microsoft.Extensions.Options;

namespace Coding.Blog.Server.HostedServices;

[ExcludeFromCodeCoverage]
internal sealed class ApplicationLifetimeService : IHostedService
{
    private readonly IHostApplicationLifetime _hostApplicationLifetime;
    private readonly TimeSpan _applicationStoppingGracePeriod;

    public ApplicationLifetimeService(
        IHostApplicationLifetime hostApplicationLifetime,
        IOptions<ApplicationLifetimeConfiguration> configurationOptions)
    {
        _hostApplicationLifetime = hostApplicationLifetime;
        _applicationStoppingGracePeriod = TimeSpan.FromSeconds(
            configurationOptions.Value.ApplicationStoppingGracePeriodSeconds
        );
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _hostApplicationLifetime.ApplicationStopping.Register(() => Thread.Sleep(_applicationStoppingGracePeriod));

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}