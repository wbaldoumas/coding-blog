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
        builder.Register(componentContext =>
            {
                var configuration = componentContext.Resolve<ResilienceConfiguration>();

                return ResiliencePolicyBuilder.Build<CosmicBooks>(
                    TimeSpan.FromMilliseconds(configuration.MedianFirstRetryDelayMilliseconds),
                    configuration.RetryCount,
                    TimeSpan.FromMilliseconds(configuration.TimeToLiveMilliseconds)
                );
            })
            .As<IAsyncPolicy<CosmicBooks>>()
            .SingleInstance();

        builder.RegisterType<CosmicClient<CosmicBooks>>()
            .As<ICosmicClient<CosmicBooks>>()
            .SingleInstance();

        builder.RegisterType<BookMapper>()
            .As<IMapper<CosmicBook, Book>>()
            .SingleInstance();
    }
}