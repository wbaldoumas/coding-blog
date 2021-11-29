using System.Diagnostics.CodeAnalysis;
using Autofac;
using Coding.Blog.Engine.Clients;
using Coding.Blog.Engine.Configurations;
using Coding.Blog.Engine.Mappers;
using Coding.Blog.Engine.Records;
using Coding.Blog.Engine.Resilience;
using Polly;

namespace Coding.Blog.Engine.Modules;

[ExcludeFromCodeCoverage]
public class CodingBlogModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        RegisterCosmicClient<CosmicBooks>(builder);
        RegisterCosmicClient<CosmicPosts>(builder);

        builder.RegisterType<BookMapper>()
            .As<IMapper<CosmicBook, Book>>()
            .SingleInstance();

        builder.RegisterType<PostMapper>()
            .As<IMapper<CosmicPost, Post>>()
            .SingleInstance();

        builder.RegisterType<PostLinker>()
            .As<IPostLinker>()
            .SingleInstance();
    }

    private static void RegisterCosmicClient<T>(ContainerBuilder builder)
    {
        builder.Register(componentContext =>
            {
                var configuration = componentContext.Resolve<ResilienceConfiguration>();

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