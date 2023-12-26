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
using PostProto = Coding.Blog.Shared.Protos.Post;

namespace Coding.Blog.Shared.Modules;

[ExcludeFromCodeCoverage]
public sealed class CodingBlogModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        RegisterCosmicClient<CosmicBooks>(builder);
        RegisterCosmicClient<CosmicPosts>(builder);
        RegisterCosmicClient<CosmicProjects>(builder);

        builder.RegisterType<CosmicPostToPostMapper>()
            .As<IMapper<CosmicPost, Post>>()
            .SingleInstance();

        builder.RegisterType<CosmicPostToPostProtoMapper>()
            .As<IMapper<CosmicPost, PostProto>>()
                       .SingleInstance();

        builder.RegisterType<CosmicBookToBookMapper>()
            .As<IMapper<CosmicBook, Book>>()
            .SingleInstance();

        builder.RegisterType<CosmicProjectToProjectMapper>()
            .As<IMapper<CosmicProject, Project>>()
            .SingleInstance();

        builder.RegisterType<PostsService>()
            .As<IPostsService>()
            .SingleInstance();

        builder.RegisterType<BooksService>()
            .As<IBooksService>()
            .SingleInstance();

        builder.RegisterType<ProjectsService>()
            .As<IProjectsService>()
            .SingleInstance();

        builder.RegisterType<ProjectsService>()
            .As<IProjectsService>()
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
