using System.Globalization;
using Coding.Blog.Configurations;
using Coding.Blog.HostedServices;
using Coding.Blog.Shared.Jobs;
using Microsoft.AspNetCore.HttpOverrides;
using Quartz;

namespace Coding.Blog.Extensions;

internal static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationLifetimeService(
        this IServiceCollection services,
        ConfigurationManager configuration)
    {
        return services
            .AddApplicationLifetimeConfiguration(configuration)
            .Configure<ForwardedHeadersOptions>(options => { options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto; })
            .Configure<HostOptions>(options =>
            {
                var applicationShutdownTimeoutSeconds = int.Parse(
                    configuration["ApplicationLifetime:ApplicationShutdownTimeoutSeconds"]!,
                    CultureInfo.InvariantCulture
                );

                options.ShutdownTimeout = TimeSpan.FromSeconds(applicationShutdownTimeoutSeconds);
            })
            .AddHostedService<ApplicationLifetimeService>();
    }

    public static IServiceCollection AddQuartzJobs(
        this IServiceCollection services,
        ConfigurationManager configuration)
    {
        services.AddQuartz(serviceCollectionQuartzConfigurator =>
        {
            serviceCollectionQuartzConfigurator
                .ConfigureJob<PostsWarmingJob>(configuration)
                .ConfigureJob<BooksWarmingJob>(configuration);
        });

        return services.AddQuartzHostedService();
    }
}
