using Coding.Blog.Shared.Configurations;
using Coding.Blog.Shared.Extensions;
using Coding.Blog.Shared.Mappers;
using Coding.Blog.Shared.Protos;
using Coding.Blog.Shared.Services;
using Grpc.Core;
using Grpc.Net.Client.Configuration;
using Grpc.Net.Client.Web;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using PostProto = Coding.Blog.Shared.Protos.Post;
using Post = Coding.Blog.Shared.Domain.Post;
using Markdig;
using Markdown.ColorCode;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddGrpcConfiguration(builder.Configuration);

builder.Services
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
    });


builder.Services.AddSingleton(_ => new MarkdownPipelineBuilder().UseAdvancedExtensions().UseColorCode().Build());

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

builder.Services.AddSingleton<IMapper<PostProto, Post>, PostProtoToPostMapper>();
builder.Services.AddSingleton<IPostLinker, PostLinker>();
builder.Services.AddSingleton<IPostsService, ClientPostsService>();

await builder.Build().RunAsync();
