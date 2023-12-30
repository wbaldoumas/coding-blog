using System.Diagnostics.CodeAnalysis;
using Autofac;
using Coding.Blog.Library.Clients;
using Coding.Blog.Library.Configurations;
using Coding.Blog.Library.Domain;
using Coding.Blog.Library.Mappers;
using Coding.Blog.Library.Records;
using Coding.Blog.Library.Resilience;
using Coding.Blog.Library.Services;
using Coding.Blog.Library.Utilities;
using Polly;
using PostProto = Coding.Blog.Library.Protos.Post;

namespace Coding.Blog.Library.Modules;

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
