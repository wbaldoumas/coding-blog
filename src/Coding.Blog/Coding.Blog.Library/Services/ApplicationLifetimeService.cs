using System.Diagnostics.CodeAnalysis;
using Coding.Blog.Library.Options;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Coding.Blog.Library.Services;

[ExcludeFromCodeCoverage]
public sealed class ApplicationLifetimeService(
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
