using Coding.Blog.Library.Mappers;
using Coding.Blog.Library.Options;
using Coding.Blog.Library.Protos;
using Coding.Blog.Library.Services;
using Coding.Blog.Library.Utilities;
using Grpc.Core;
using Grpc.Net.Client.Configuration;
using Grpc.Net.Client.Web;
using Markdig;
using Markdown.ColorCode;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
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
            .AddGrpcClient<Posts.PostsClient>((serviceProvider, grpcClientFactoryOptions) =>
            {
                grpcClientFactoryOptions.Address = new Uri(serviceProvider.GetRequiredService<NavigationManager>().BaseUri);
            })
            .ConfigurePrimaryHttpMessageHandler(() => new GrpcWebHandler(GrpcWebMode.GrpcWeb, new HttpClientHandler()))
            .ConfigureChannel((serviceProvider, grpcChannelOptions) => { grpcChannelOptions.ServiceConfig = serviceProvider.GetRequiredService<ServiceConfig>(); });

        return services;
    }
}
