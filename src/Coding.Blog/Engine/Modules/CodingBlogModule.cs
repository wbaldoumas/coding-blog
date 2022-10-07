using System.Diagnostics.CodeAnalysis;
using Autofac;
using Coding.Blog.Engine.Clients;
using Coding.Blog.Engine.Configurations;
using Coding.Blog.Engine.Mappers;
using Coding.Blog.Engine.Records;
using Coding.Blog.Engine.Resilience;
using Coding.Blog.Engine.Utilities;
using Markdig;
using Microsoft.Extensions.Options;
using Polly;

namespace Coding.Blog.Engine.Modules;

[ExcludeFromCodeCoverage]
public sealed class CodingBlogModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        RegisterCosmicClient<CosmicBooks>(builder);
        RegisterCosmicClient<CosmicPosts>(builder);

        builder.RegisterType<BookMapper>()
            .As<IMapper<CosmicBook, Book>>()
            .SingleInstance();

        builder.RegisterType<StringSanitizer>()
            .As<IStringSanitizer>()
            .SingleInstance();

        builder.RegisterType<ReadTimeEstimator>()
            .As<IReadTimeEstimator>()
            .SingleInstance();

        builder.RegisterType<PostMapper>()
            .As<IMapper<CosmicPost, Post>>()
            .SingleInstance();

        builder.RegisterType<PostLinker>()
            .As<IPostLinker>()
            .SingleInstance();

        builder.Register(_ => new MarkdownPipelineBuilder().UseAdvancedExtensions().Build())
            .AsSelf()
            .SingleInstance();
    }

    private static void RegisterCosmicClient<T>(ContainerBuilder builder)
    {
        builder.Register(componentContext =>
            {
                var configuration = componentContext.Resolve<IOptions<ResilienceConfiguration>>().Value;

                return ResiliencePolicyBuilder.Build<T>(
                    TimeSpan.FromMilliseconds(configuration.MedianFirstRetryDelayMilliseconds),
                    configuration.RetryCount,
                    TimeSpan.FromMilliseconds(configuration.TimeToLiveMilliseconds)
                );
            })
            .As<IAsyncPolicy<T>>()
            .SingleInstance();

        builder.RegisterType<CosmicClient<T>>()
            .As<ICosmicClient<T>>()
            .SingleInstance();
    }
}