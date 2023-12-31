using Blazorise;
using Blazorise.Icons.FontAwesome;
using Coding.Blog.Library.Clients;
using Coding.Blog.Library.Configurations;
using Coding.Blog.Library.Domain;
using Coding.Blog.Library.Jobs;
using Coding.Blog.Library.Mappers;
using Coding.Blog.Library.Records;
using Coding.Blog.Library.Resilience;
using Coding.Blog.Library.Services;
using Coding.Blog.Library.Utilities;
using Markdig;
using Markdown.ColorCode;
using Microsoft.AspNetCore.HttpOverrides;
using Quartz;
using System.Configuration;
using System.Globalization;
using Post = Coding.Blog.Library.Domain.Post;
using PostProto = Coding.Blog.Library.Protos.Post;

namespace Coding.Blog.Extensions;

internal static class ServiceCollectionExtensions
{
    /// <summary>
    ///     Configure the necessary services for the application.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configuration">The configuration.</param>
    /// <returns>The configured service collection.</returns>
    public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddConfigurations(configuration)
            .AddApplicationLifetimeService(configuration)
            .AddQuartzJobs(configuration)
            .AddMappers()
            .AddCosmicClients()
            .AddDomainServices()
            .AddUtilities()
            .AddBlazorise()
            .AddEmptyProviders()
            .AddFontAwesomeIcons()
            .AddRazorComponents()
            .AddInteractiveServerComponents()
            .AddInteractiveWebAssemblyComponents();

        services.AddHealthChecks();

        services.AddGrpc();

        services.AddSignalR(hubOptions =>
        {
            hubOptions.MaximumReceiveMessageSize = 1024000;
            hubOptions.KeepAliveInterval = TimeSpan.FromSeconds(1);
            hubOptions.EnableDetailedErrors = true;
            hubOptions.HandshakeTimeout = TimeSpan.FromSeconds(30);
            hubOptions.ClientTimeoutInterval = TimeSpan.FromMinutes(30);
            hubOptions.StreamBufferCapacity = 20;
            hubOptions.StatefulReconnectBufferSize = 1024000;
        });

        return services;
    }

    private static IServiceCollection AddMappers(this IServiceCollection services) => services
        .AddSingleton<IMapper<CosmicPost, Post>, CosmicPostToPostMapper>()
        .AddSingleton<IMapper<CosmicPost, PostProto>, CosmicPostToPostProtoMapper>()
        .AddSingleton<IMapper<CosmicBook, Book>, CosmicBookToBookMapper>()
        .AddSingleton<IMapper<CosmicProject, Project>, CosmicProjectToProjectMapper>();

    private static IServiceCollection AddCosmicClients(this IServiceCollection services) => services
        .AddCosmicClient<CosmicPosts>()
        .AddCosmicClient<CosmicBooks>()
        .AddCosmicClient<CosmicProjects>();

    private static IServiceCollection AddCosmicClient<T>(this IServiceCollection services)
    {
        return services
            .AddSingleton(serviceProvider =>
            {
                var resilienceConfiguration = serviceProvider.GetRequiredService<ResilienceConfiguration>();

                return ResiliencePolicyBuilder.Build<T>(
                    TimeSpan.FromMilliseconds(resilienceConfiguration.MedianFirstRetryDelayMilliseconds),
                    resilienceConfiguration.RetryCount,
                    TimeSpan.FromMilliseconds(resilienceConfiguration.TimeToLiveMilliseconds)
                );
            })
            .AddSingleton<ICosmicClient<T>, CosmicClient<T>>();
    }

    private static IServiceCollection AddDomainServices(this IServiceCollection services) => services
        .AddSingleton<IPostsService, PostsService>()
        .AddSingleton<IBooksService, BooksService>()
        .AddSingleton<IProjectsService, ProjectsService>();

    private static IServiceCollection AddUtilities(this IServiceCollection services) => services
        .AddSingleton(_ => new MarkdownPipelineBuilder().UseAdvancedExtensions().UseColorCode().Build())
        .AddSingleton<IStringSanitizer, StringSanitizer>()
        .AddSingleton<IPostLinker, PostLinker>()
        .AddSingleton<IReadTimeEstimator, ReadTimeEstimator>();

    private static IServiceCollection AddApplicationLifetimeService(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        return services
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

    private static IServiceCollection AddConfigurations(this IServiceCollection services, IConfiguration configuration) => services
        .AddCosmicConfiguration(configuration)
        .AddResilienceConfiguration(configuration)
        .AddApplicationLifetimeConfiguration(configuration);

    private static IServiceCollection AddApplicationLifetimeConfiguration(
        this IServiceCollection serviceCollection,
        IConfiguration configuration
    ) => serviceCollection.AddSingleton(
        new ApplicationLifetimeConfiguration
        {
            ApplicationStoppingGracePeriodSeconds = int.Parse(configuration["ApplicationLifetime:ApplicationStoppingGracePeriodSeconds"]!, CultureInfo.InvariantCulture)
        }
    );

    private static IServiceCollection AddCosmicConfiguration(
        this IServiceCollection serviceCollection,
        IConfiguration configuration
    ) => serviceCollection.AddSingleton(
        new CosmicConfiguration
        {
            Endpoint = configuration["Cosmic:Endpoint"]!,
            BucketSlug = configuration["Cosmic:BucketSlug"]!,
            ReadKey = configuration["Cosmic:ReadKey"]!
        }
    );

    private static IServiceCollection AddResilienceConfiguration(
        this IServiceCollection serviceCollection,
        IConfiguration configuration
    ) => serviceCollection.AddSingleton(
        new ResilienceConfiguration
        {
            MedianFirstRetryDelayMilliseconds = int.Parse(configuration["Resilience:MedianFirstRetryDelayMilliseconds"]!, CultureInfo.InvariantCulture),
            RetryCount = int.Parse(configuration["Resilience:RetryCount"]!, CultureInfo.InvariantCulture),
            TimeToLiveMilliseconds = int.Parse(configuration["Resilience:TimeToLiveMilliseconds"]!, CultureInfo.InvariantCulture)
        }
    );

    private static IServiceCollection AddQuartzJobs(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddQuartz(serviceCollectionQuartzConfigurator =>
        {
            serviceCollectionQuartzConfigurator
                .ConfigureJob<PostsWarmingJob>(configuration)
                .ConfigureJob<BooksWarmingJob>(configuration)
                .ConfigureJob<ProjectsWarmingJob>(configuration);
        });

        return services.AddQuartzHostedService();
    }

    private static IServiceCollectionQuartzConfigurator ConfigureJob<T>(
        this IServiceCollectionQuartzConfigurator serviceCollectionQuartzConfigurator,
        IConfiguration configuration)
        where T : IJob
    {
        var jobName = typeof(T).Name;
        var intervalSeconds = GetQuartzIntervalSeconds(jobName, configuration);
        var jobKey = new JobKey(jobName);

        serviceCollectionQuartzConfigurator.AddJob<T>(jobKey);

        return serviceCollectionQuartzConfigurator.AddTrigger(triggerConfigurator => triggerConfigurator
            .ForJob(jobKey)
            .WithSimpleSchedule(simpleScheduleBuilder => simpleScheduleBuilder
                .WithIntervalInSeconds(intervalSeconds)
                .RepeatForever()
            )
            .StartNow()
        );
    }

    private static int GetQuartzIntervalSeconds(string jobName, IConfiguration configuration)
    {
        var configurationKey = $"Quartz:{jobName}:IntervalSeconds";
        var intervalSecondsConfiguration = configuration[configurationKey];

        if (string.IsNullOrEmpty(intervalSecondsConfiguration))
        {
            throw new ConfigurationErrorsException($"No job interval configuration found for {configurationKey}.");
        }

        if (!int.TryParse(intervalSecondsConfiguration, NumberStyles.Integer, CultureInfo.InvariantCulture, out var intervalSeconds) || intervalSeconds <= 0)
        {
            throw new ConfigurationErrorsException(
                $"Invalid job interval configuration found for {configurationKey}. Job interval configuration: {intervalSecondsConfiguration}."
            );
        }

        return intervalSeconds;
    }
}
