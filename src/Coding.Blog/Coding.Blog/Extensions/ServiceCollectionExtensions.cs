using Blazorise;
using Blazorise.Icons.FontAwesome;
using Coding.Blog.Library.Clients;
using Coding.Blog.Library.Jobs;
using Coding.Blog.Library.Mappers;
using Coding.Blog.Library.Options;
using Coding.Blog.Library.Records;
using Coding.Blog.Library.Resilience;
using Coding.Blog.Library.Services;
using Coding.Blog.Library.Utilities;
using ColorCode;
using Markdig;
using Markdown.ColorCode;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Options;
using Quartz;
using Book = Coding.Blog.Library.Domain.Book;
using Post = Coding.Blog.Library.Domain.Post;
using Project = Coding.Blog.Library.Domain.Project;
using ProtoBook = Coding.Blog.Library.Protos.Book;
using ProtoPost = Coding.Blog.Library.Protos.Post;
using ProtoProject = Coding.Blog.Library.Protos.Project;
using QuartzOptions = Coding.Blog.Library.Options.QuartzOptions;

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
            .AddOptions(configuration)
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

    private static IServiceCollection AddMappers(this IServiceCollection services) => services
        .AddSingleton<IMapper<CosmicPost, Post>, CosmicPostToPostMapper>()
        .AddSingleton<IMapper<CosmicPost, ProtoPost>, CosmicPostToProtoPostMapper>()
        .AddSingleton<IMapper<CosmicBook, Book>, CosmicBookToBookMapper>()
        .AddSingleton<IMapper<CosmicBook, ProtoBook>, CosmicBookToProtoBookMapper>()
        .AddSingleton<IMapper<CosmicProject, Project>, CosmicProjectToProjectMapper>()
        .AddSingleton<IMapper<CosmicProject, ProtoProject>, CosmicProjectToProtoProjectMapper>();

    private static IServiceCollection AddCosmicClients(this IServiceCollection services) => services
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

    private static IServiceCollection AddDomainServices(this IServiceCollection services) => services
        .AddSingleton<IBlogService<IEnumerable<Post>>, BlogService<CosmicPost, Post>>()
        .AddSingleton<IBlogService<IEnumerable<Book>>, BlogService<CosmicBook, Book>>()
        .AddSingleton<IBlogService<IEnumerable<Project>>, BlogService<CosmicProject, Project>>()
        .AddSingleton<IApplicationStateService<Post>, ApplicationStateService<Post>>()
        .AddSingleton<IApplicationStateService<Book>, ApplicationStateService<Book>>()
        .AddSingleton<IApplicationStateService<Project>, ApplicationStateService<Project>>();

    private static IServiceCollection AddUtilities(this IServiceCollection services) => services
        .AddSingleton(_ => new MarkdownPipelineBuilder()
            .UseAdvancedExtensions()
            .UseColorCode(
                HtmlFormatterType.Style,
                SyntaxHighlighting.Dark,
                new List<ILanguage> { new CSharpOverride() })
            .Build())
        .AddSingleton<IStringSanitizer, StringSanitizer>()
        .AddSingleton<IPostLinker, PostLinker>()
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

        services.AddOptions<QuartzOptions>()
            .Bind(configuration.GetSection(QuartzOptions.Key))
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
        var quartzOptions = configuration.GetSection(QuartzOptions.Key).Get<QuartzOptions>();

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
