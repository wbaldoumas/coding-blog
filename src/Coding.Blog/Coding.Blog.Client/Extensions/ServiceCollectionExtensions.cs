﻿using Blazorise;
using Blazorise.Bootstrap5;
using Blazorise.Icons.FontAwesome;
using Coding.Blog.Client.Clients;
using Coding.Blog.Client.Options;
using Coding.Blog.Client.Services;
using Coding.Blog.Client.Utilities;
using Coding.Blog.Library.Protos;
using Coding.Blog.Library.Services;
using Coding.Blog.Library.Utilities;
using Grpc.Core;
using Grpc.Net.Client.Configuration;
using Grpc.Net.Client.Web;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using System.Diagnostics.CodeAnalysis;
using Book = Coding.Blog.Library.Domain.Book;
using Post = Coding.Blog.Library.Domain.Post;
using Project = Coding.Blog.Library.Domain.Project;
using ProtoBook = Coding.Blog.Library.Protos.Book;
using ProtoPost = Coding.Blog.Library.Protos.Post;
using ProtoProject = Coding.Blog.Library.Protos.Project;

namespace Coding.Blog.Client.Extensions;

[ExcludeFromCodeCoverage]
internal static class ServiceCollectionExtensions
{
    /// <summary>
    ///     Configure the necessary services for the application.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configuration">The configuration.</param>
    /// <returns>The configured service collection.</returns>
    public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration configuration) => services
        .AddBlazorise()
        .AddBootstrap5Providers()
        .AddFontAwesomeIcons()
        .AddSingleton<IMapper, Mapper>()
        .AddSingleton<IPostLinker, PostLinker>()
        .AddSingleton<IProtoClient<ProtoPost>, PostsClient>()
        .AddSingleton<IProtoClient<ProtoBook>, BooksClient>()
        .AddSingleton<IProtoClient<ProtoProject>, ProjectsClient>()
        .AddSingleton<IBlogService<Post>, BlogService<ProtoPost, Post>>()
        .AddSingleton<IBlogService<Book>, BlogService<ProtoBook, Book>>()
        .AddSingleton<IBlogService<Project>, BlogService<ProtoProject, Project>>()
        .AddSingleton<IPersistentComponentStateService<Post>, PersistentComponentStateService<Post>>()
        .AddSingleton<IPersistentComponentStateService<Book>, PersistentComponentStateService<Book>>()
        .AddSingleton<IPersistentComponentStateService<Project>, PersistentComponentStateService<Project>>()
        .AddSingleton<IJSInteropService, JSInteropService>()
        .AddGrpc(configuration);

    private static IServiceCollection AddGrpc(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<GrpcOptions>()
            .Bind(configuration.GetSection(GrpcOptions.Key))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddSingleton(serviceProvider =>
        {
            var grpcOptions = serviceProvider.GetRequiredService<IOptions<GrpcOptions>>();

            return new ServiceConfig
            {
                MethodConfigs =
                {
                    new MethodConfig
                    {
                        Names = { MethodName.Default },
                        RetryPolicy = new RetryPolicy
                        {
                            MaxAttempts = grpcOptions.Value.MaxAttempts,
                            InitialBackoff = grpcOptions.Value.InitialBackoff,
                            MaxBackoff = grpcOptions.Value.MaxBackoff,
                            BackoffMultiplier = grpcOptions.Value.BackoffMultiplier,
                            RetryableStatusCodes = { StatusCode.Unavailable }
                        }
                    }
                }
            };
        })
        .AddConfiguredGrpcClient<Posts.PostsClient>()
        .AddConfiguredGrpcClient<Books.BooksClient>()
        .AddConfiguredGrpcClient<Projects.ProjectsClient>();

        return services;
    }

    private static IServiceCollection AddConfiguredGrpcClient<T>(this IServiceCollection services) where T : ClientBase
    {
        services.AddGrpcClient<T>((serviceProvider, grpcClientFactoryOptions) =>
        {
            grpcClientFactoryOptions.Address = new Uri(serviceProvider.GetRequiredService<NavigationManager>().BaseUri);
        })
        .ConfigurePrimaryHttpMessageHandler(() => new GrpcWebHandler(GrpcWebMode.GrpcWeb, new HttpClientHandler()))
        .ConfigureChannel((serviceProvider, grpcChannelOptions) =>
        {
            grpcChannelOptions.ServiceConfig = serviceProvider.GetRequiredService<ServiceConfig>();
        });

        return services;
    }
}
