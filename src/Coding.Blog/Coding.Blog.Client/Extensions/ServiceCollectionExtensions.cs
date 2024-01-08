using Coding.Blog.Library.Mappers;
using Coding.Blog.Library.Options;
using Coding.Blog.Library.Protos;
using Coding.Blog.Library.Services;
using Coding.Blog.Library.Utilities;
using ColorCode;
using Grpc.Core;
using Grpc.Net.Client.Configuration;
using Grpc.Net.Client.Web;
using Markdig;
using Markdown.ColorCode;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using Book = Coding.Blog.Library.Domain.Book;
using Post = Coding.Blog.Library.Domain.Post;
using Project = Coding.Blog.Library.Domain.Project;
using ProtoBook = Coding.Blog.Library.Protos.Book;
using ProtoPost = Coding.Blog.Library.Protos.Post;
using ProtoProject = Coding.Blog.Library.Protos.Project;

namespace Coding.Blog.Client.Extensions;

internal static class ServiceCollectionExtensions
{
    public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration configuration) => services
        .AddSingleton(_ => new MarkdownPipelineBuilder()
            .UseAdvancedExtensions()
            .UseColorCode(
                HtmlFormatterType.Style,
                SyntaxHighlighting.Dark,
                new List<ILanguage> { new CSharpOverride() })
            .Build())
        .AddSingleton<IMapper<ProtoPost, Post>, ProtoPostToPostMapper>()
        .AddSingleton<IMapper<ProtoBook, Book>, ProtoBookToBookMapper>()
        .AddSingleton<IMapper<ProtoProject, Project>, ProtoProjectToProjectMapper>()
        .AddSingleton<IPostLinker, PostLinker>()
        .AddSingleton<IPostsService, ClientPostsService>()
        .AddSingleton<IProjectsService, ClientProjectsService>()
        .AddSingleton<IBooksService, ClientBooksService>()
        .AddSingleton<IPersistentService<IDictionary<string, Post>>, PersistentPostsService>()
        .AddSingleton<IPersistentService<IList<Book>>, PersistentBooksService>()
        .AddSingleton<IPersistentService<IList<Project>>, PersistentProjectsService>()
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
