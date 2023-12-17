using System.Diagnostics.CodeAnalysis;
using Autofac;
using Coding.Blog.Shared.Clients;
using Coding.Blog.Shared.Configurations;
using Coding.Blog.Shared.Domain;
using Coding.Blog.Shared.Mappers;
using Coding.Blog.Shared.Records;
using Coding.Blog.Shared.Resilience;
using Coding.Blog.Shared.Services;
using Coding.Blog.Shared.Utilities;
using Polly;

namespace Coding.Blog.Shared.Modules;

[ExcludeFromCodeCoverage]
public sealed class CodingBlogModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        RegisterCosmicClient<CosmicBooks>(builder);
        RegisterCosmicClient<CosmicPosts>(builder);

        builder.RegisterType<CosmicPostMapper>()
            .As<IMapper<CosmicPost, Post>>()
            .SingleInstance();

        builder.RegisterType<CosmicBookMapper>()
            .As<IMapper<CosmicBook, Book>>()
            .SingleInstance();

        builder.RegisterType<PostsService>()
            .As<IPostsService>()
            .SingleInstance();

        builder.RegisterType<BooksService>()
            .As<IBooksService>()
            .SingleInstance();

        builder.RegisterType<StringSanitizer>()
            .As<IStringSanitizer>()
            .SingleInstance();

        builder.RegisterType<PostLinker>()
            .As<IPostLinker>()
            .SingleInstance();

        builder.RegisterType<ReadTimeEstimator>()
            .As<IReadTimeEstimator>()
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
