using System.Diagnostics.CodeAnalysis;

namespace Coding.Blog.Server.HostedServices;

[ExcludeFromCodeCoverage]
internal sealed class ApplicationLifetimeService : IHostedService
{
    private readonly IHostApplicationLifetime _hostApplicationLifetime;
    private readonly TimeSpan _applicationStoppingGracePeriod;

    public ApplicationLifetimeService(IHostApplicationLifetime hostApplicationLifetime)
    {
        _hostApplicationLifetime = hostApplicationLifetime;
        _applicationStoppingGracePeriod = TimeSpan.FromSeconds(30);
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _hostApplicationLifetime.ApplicationStopping.Register(() => Thread.Sleep(_applicationStoppingGracePeriod));

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}