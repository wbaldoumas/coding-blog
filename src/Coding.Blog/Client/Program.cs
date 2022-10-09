using Coding.Blog.Client;
using Coding.Blog.Engine;
using Coding.Blog.Engine.Clients;
using Coding.Blog.Engine.Configurations;
using Coding.Blog.Engine.Extensions;
using Coding.Blog.Engine.Resilience;
using Grpc.Core;
using Grpc.Net.Client.Configuration;
using Grpc.Net.Client.Web;
using Markdig;
using Markdown.ColorCode;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services
    .AddResilienceConfiguration(builder.Configuration)
    .AddGrpcResilienceConfiguration(builder.Configuration)
    .AddSingleton(serviceProvider =>
    {
        var grpcResilienceConfiguration = serviceProvider.GetRequiredService<GrpcResilienceConfiguration>();

        return new ServiceConfig
        {
            MethodConfigs =
            {
                new MethodConfig
                {
                    Names = { MethodName.Default },
                    RetryPolicy = new RetryPolicy
                    {
                        MaxAttempts = grpcResilienceConfiguration.MaxAttempts,
                        InitialBackoff =
                            TimeSpan.FromMilliseconds(grpcResilienceConfiguration.InitialBackoffMilliseconds),
                        MaxBackoff = TimeSpan.FromMilliseconds(grpcResilienceConfiguration.MaxBackoffMilliseconds),
                        BackoffMultiplier = grpcResilienceConfiguration.BackoffMultiplier,
                        RetryableStatusCodes = { StatusCode.Unavailable }
                    }
                }
            }
        };
    })
    .AddSingleton<IResilientClient<Book>, ResilientBooksClient>(serviceProvider =>
    {
        var configuration = serviceProvider.GetRequiredService<ResilienceConfiguration>();
        var booksClient = serviceProvider.GetRequiredService<Books.BooksClient>();
        var logger = serviceProvider.GetRequiredService<ILogger<Book>>();

        var resiliencePolicy = ResiliencePolicyBuilder.Build<IEnumerable<Book>>(
            TimeSpan.FromMilliseconds(configuration.MedianFirstRetryDelayMilliseconds),
            configuration.RetryCount,
            TimeSpan.FromMilliseconds(configuration.TimeToLiveMilliseconds)
        );

        return new ResilientBooksClient(booksClient, logger, resiliencePolicy);
    })
    .AddSingleton<IResilientClient<Post>, ResilientPostsClient>(serviceProvider =>
    {
        var configuration = serviceProvider.GetRequiredService<ResilienceConfiguration>();
        var booksClient = serviceProvider.GetRequiredService<Posts.PostsClient>();
        var logger = serviceProvider.GetRequiredService<ILogger<Post>>();

        var resiliencePolicy = ResiliencePolicyBuilder.Build<IEnumerable<Post>>(
            TimeSpan.FromMilliseconds(configuration.MedianFirstRetryDelayMilliseconds),
            configuration.RetryCount,
            TimeSpan.FromMilliseconds(configuration.TimeToLiveMilliseconds)
        );

        return new ResilientPostsClient(booksClient, logger, resiliencePolicy);
    })
    .AddSingleton(_ => new MarkdownPipelineBuilder().UseAdvancedExtensions().UseColorCode().Build());

builder.Services
    .AddGrpcClient<Books.BooksClient>((serviceProvider, grpcClientFactoryOptions) =>
    {
        grpcClientFactoryOptions.Address = new Uri(serviceProvider.GetRequiredService<NavigationManager>().BaseUri);
    })
    .ConfigurePrimaryHttpMessageHandler(() => new GrpcWebHandler(GrpcWebMode.GrpcWeb, new HttpClientHandler()))
    .ConfigureChannel((serviceProvider, grpcChannelOptions) =>
    {
        grpcChannelOptions.ServiceConfig = serviceProvider.GetRequiredService<ServiceConfig>();
    });

builder.Services
    .AddGrpcClient<Posts.PostsClient>((serviceProvider, grpcClientFactoryOptions) =>
    {
        grpcClientFactoryOptions.Address = new Uri(serviceProvider.GetRequiredService<NavigationManager>().BaseUri);
    })
    .ConfigurePrimaryHttpMessageHandler(() => new GrpcWebHandler(GrpcWebMode.GrpcWeb, new HttpClientHandler()))
    .ConfigureChannel((serviceProvider, grpcChannelOptions) =>
    {
        grpcChannelOptions.ServiceConfig = serviceProvider.GetRequiredService<ServiceConfig>();
    });

await builder
    .Build()
    .RunAsync()
    .ConfigureAwait(false);