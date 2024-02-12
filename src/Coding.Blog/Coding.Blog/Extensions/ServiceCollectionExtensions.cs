using System.Diagnostics.CodeAnalysis;
using Blazorise;
using Blazorise.Bootstrap5;
using Blazorise.Icons.FontAwesome;
using Coding.Blog.Clients;
using Coding.Blog.DataTransfer;
using Coding.Blog.DataTransfer.PostProcessors;
using Coding.Blog.Jobs;
using Coding.Blog.Library.Domain;
using Coding.Blog.Library.Services;
using Coding.Blog.Library.Utilities;
using Coding.Blog.Options;
using Coding.Blog.Services;
using Coding.Blog.Utilities;
using ColorCode;
using Markdig;
using Markdown.ColorCode;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Options;
using Quartz;

namespace Coding.Blog.Extensions;

[ExcludeFromCodeCoverage]
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
            .AddOptions(configuration)
            .AddApplicationLifetimeService(configuration)
            .AddQuartzJobs(configuration)
            .AddCosmicClients()
            .AddServices()
            .AddUtilities()
            .AddBlazorise()
            .AddBootstrap5Providers()
            .AddFontAwesomeIcons()
            .AddResponseCaching()
            .AddResponseCompression()
            .AddRazorComponents()
            .AddInteractiveServerComponents()
            .AddInteractiveWebAssemblyComponents();

        services.AddControllers();
        services.AddHealthChecks();
        services.AddGrpc();

        var signalROptions = configuration.GetSection(SignalROptions.Key).Get<SignalROptions>();

        services.AddSignalR(hubOptions =>
        {
            hubOptions.MaximumReceiveMessageSize = signalROptions!.MaximumReceiveMessageSize;
            hubOptions.KeepAliveInterval = signalROptions.KeepAliveInterval;
            hubOptions.EnableDetailedErrors = signalROptions.EnableDetailedErrors;
            hubOptions.HandshakeTimeout = signalROptions.HandshakeTimeout;
            hubOptions.ClientTimeoutInterval = signalROptions.ClientTimeoutInterval;
            hubOptions.StreamBufferCapacity = signalROptions.StreamBufferCapacity;
            hubOptions.StatefulReconnectBufferSize = signalROptions.StatefulReconnectBufferSize;
        });

        return services;
    }

    private static IServiceCollection AddCosmicClients(this IServiceCollection services) => services
        .AddSingleton<ICosmicObjectPostProcessor<CosmicBook>, CosmicBookPostProcessor>()
        .AddSingleton<ICosmicObjectPostProcessor<CosmicPost>, CosmicPostPostProcessor>()
        .AddSingleton<ICosmicObjectPostProcessor<CosmicProject>, CosmicProjectPostProcessor>()
        .AddCosmicClient<CosmicPost>()
        .AddCosmicClient<CosmicBook>()
        .AddCosmicClient<CosmicProject>();

    private static IServiceCollection AddCosmicClient<T>(this IServiceCollection services)
    {
        return services
            .AddSingleton(serviceProvider =>
            {
                var resilienceOptions = serviceProvider.GetRequiredService<IOptions<ResilienceOptions>>();

                return ResiliencePolicyBuilder.Build<IEnumerable<T>>(
                    TimeSpan.FromMilliseconds(resilienceOptions.Value.MedianFirstRetryDelayMilliseconds),
                    resilienceOptions.Value.RetryCount,
                    TimeSpan.FromMilliseconds(resilienceOptions.Value.TimeToLiveMilliseconds)
                );
            })
            .AddSingleton<ICosmicClient<T>, CosmicClient<T>>();
    }

    private static IServiceCollection AddServices(this IServiceCollection services) => services
        .AddSingleton<ISyndicationFeedService, SyndicationFeedService>()
        .AddSingleton<IBlogService<Post>, BlogService<CosmicPost, Post>>()
        .AddSingleton<IBlogService<Book>, BlogService<CosmicBook, Book>>()
        .AddSingleton<IBlogService<Project>, BlogService<CosmicProject, Project>>()
        .AddScoped<IJSInteropService, JSInteropService>()
        .AddSingleton<IPersistentComponentStateService<Post>, PersistentComponentStateService<Post>>()
        .AddSingleton<IPersistentComponentStateService<Book>, PersistentComponentStateService<Book>>()
        .AddSingleton<IPersistentComponentStateService<Project>, PersistentComponentStateService<Project>>();

    private static IServiceCollection AddUtilities(this IServiceCollection services) => services
        .AddSingleton(_ => new MarkdownPipelineBuilder()
            .UseAdvancedExtensions()
            .UseColorCode(
                HtmlFormatterType.Style,
                SyntaxHighlighting.Dark,
                new List<ILanguage> { new CSharpOverride() })
            .Build())
        .AddSingleton<IPostToSyndicationItemMapper, PostToSyndicationItemMapper>()
        .AddSingleton<IStringSanitizer, StringSanitizer>()
        .AddSingleton<IPostLinker, PostLinker>()
        .AddSingleton<IMapper, Mapper>()
        .AddSingleton<IReadTimeEstimator, ReadTimeEstimator>();

    private static IServiceCollection AddApplicationLifetimeService(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var applicationLifetimeOptions = configuration.GetSection(ApplicationLifetimeOptions.Key).Get<ApplicationLifetimeOptions>();

        return services
            .Configure<ForwardedHeadersOptions>(options => { options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto; })
            .Configure<HostOptions>(options =>
            {
                options.ShutdownTimeout = TimeSpan.FromSeconds(applicationLifetimeOptions!.ApplicationStoppingGracePeriodSeconds);
            })
            .AddHostedService<ApplicationLifetimeService>();
    }

    private static IServiceCollection AddOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<ApplicationInfoOptions>()
            .Bind(configuration.GetSection(ApplicationInfoOptions.Key))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddOptions<CosmicOptions>()
            .Bind(configuration.GetSection(CosmicOptions.Key))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddOptions<ResilienceOptions>()
            .Bind(configuration.GetSection(ResilienceOptions.Key))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddOptions<ApplicationLifetimeOptions>()
            .Bind(configuration.GetSection(ApplicationLifetimeOptions.Key))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddOptions<CacheWarmingJobOptions>()
            .Bind(configuration.GetSection(CacheWarmingJobOptions.Key))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddOptions<SignalROptions>()
            .Bind(configuration.GetSection(SignalROptions.Key))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        return services;
    }

    private static IServiceCollection AddQuartzJobs(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var quartzOptions = configuration.GetSection(CacheWarmingJobOptions.Key).Get<CacheWarmingJobOptions>();

        services.AddQuartz(serviceCollectionQuartzConfigurator =>
        {
            serviceCollectionQuartzConfigurator
                .ConfigureJob<CacheWarmingJob<CosmicPost>>(quartzOptions!.PostsWarmingJob)
                .ConfigureJob<CacheWarmingJob<CosmicBook>>(quartzOptions.BooksWarmingJob)
                .ConfigureJob<CacheWarmingJob<CosmicProject>>(quartzOptions.ProjectsWarmingJob);
        });

        return services.AddQuartzHostedService();
    }

    private static IServiceCollectionQuartzConfigurator ConfigureJob<T>(
        this IServiceCollectionQuartzConfigurator serviceCollectionQuartzConfigurator,
        QuartzJobOptions jobOptions)
        where T : IJob
    {
        var jobKey = new JobKey(jobOptions.JobKey);

        serviceCollectionQuartzConfigurator.AddJob<T>(jobKey);

        return serviceCollectionQuartzConfigurator.AddTrigger(triggerConfigurator => triggerConfigurator
            .ForJob(jobKey)
            .WithSimpleSchedule(simpleScheduleBuilder => simpleScheduleBuilder
                .WithIntervalInSeconds(jobOptions.IntervalSeconds)
                .RepeatForever()
            )
            .StartNow()
        );
    }
}
