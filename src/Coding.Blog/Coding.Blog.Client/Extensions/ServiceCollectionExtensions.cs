using Coding.Blog.Library.Configurations;
using System.Globalization;
using Coding.Blog.Library.Protos;
using Grpc.Core;
using Grpc.Net.Client.Configuration;
using Grpc.Net.Client.Web;
using Microsoft.AspNetCore.Components;
using Coding.Blog.Library.Mappers;
using Coding.Blog.Library.Services;
using Markdig;
using Markdown.ColorCode;
using Post = Coding.Blog.Library.Domain.Post;
using PostProto = Coding.Blog.Library.Protos.Post;

namespace Coding.Blog.Client.Extensions;

internal static class ServiceCollectionExtensions
{
    public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration configuration) => services
        .AddSingleton(_ => new MarkdownPipelineBuilder().UseAdvancedExtensions().UseColorCode().Build())
        .AddSingleton<IMapper<PostProto, Post>, PostProtoToPostMapper>()
        .AddSingleton<IPostLinker, PostLinker>()
        .AddSingleton<IPostsService, ClientPostsService>()
        .AddGrpc(configuration);

    private static IServiceCollection AddGrpc(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddGrpcConfiguration(configuration)
            .AddSingleton(serviceProvider =>
            {
                var grpcConfiguration = serviceProvider.GetRequiredService<GrpcConfiguration>();

                return new ServiceConfig
                {
                    MethodConfigs =
                    {
                        new MethodConfig
                        {
                            Names = { MethodName.Default },
                            RetryPolicy = new RetryPolicy
                            {
                                MaxAttempts = grpcConfiguration.MaxAttempts,
                                InitialBackoff = grpcConfiguration.InitialBackoff,
                                MaxBackoff = grpcConfiguration.MaxBackoff,
                                BackoffMultiplier = grpcConfiguration.BackoffMultiplier,
                                RetryableStatusCodes = { StatusCode.Unavailable }
                            }
                        }
                    }
                };
            })
            .AddGrpcClient<Posts.PostsClient>((serviceProvider, grpcClientFactoryOptions) =>
            {
                grpcClientFactoryOptions.Address = new Uri(serviceProvider.GetRequiredService<NavigationManager>().BaseUri);
            })
            .ConfigurePrimaryHttpMessageHandler(() => new GrpcWebHandler(GrpcWebMode.GrpcWeb, new HttpClientHandler()))
            .ConfigureChannel((serviceProvider, grpcChannelOptions) => { grpcChannelOptions.ServiceConfig = serviceProvider.GetRequiredService<ServiceConfig>(); });

        return services;
    }

    private static IServiceCollection AddGrpcConfiguration(
        this IServiceCollection serviceCollection,
        IConfiguration configuration
    ) => serviceCollection.AddSingleton(
        new GrpcConfiguration
        {
            MaxAttempts = int.Parse(configuration["Grpc:MaxAttempts"]!, CultureInfo.InvariantCulture),
            InitialBackoffMilliseconds = int.Parse(configuration["Grpc:InitialBackoffMilliseconds"]!, CultureInfo.InvariantCulture),
            MaxBackoffMilliseconds = int.Parse(configuration["Grpc:MaxBackoffMilliseconds"]!, CultureInfo.InvariantCulture),
            BackoffMultiplier = int.Parse(configuration["Grpc:BackoffMultiplier"]!, CultureInfo.InvariantCulture)
        }
    );
}
