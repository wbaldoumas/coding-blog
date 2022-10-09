using System.Diagnostics.CodeAnalysis;
using Coding.Blog.Engine.Configurations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Coding.Blog.Engine.Extensions;

[ExcludeFromCodeCoverage]
public static class CosmicConfigurationExtensions
{
    public static IServiceCollection AddCosmicConfiguration(
        this IServiceCollection serviceCollection,
        IConfiguration configuration
    ) => serviceCollection.AddSingleton(
        new CosmicConfiguration
        {
            Endpoint = configuration["Cosmic:Endpoint"],
            BucketSlug = configuration["Cosmic:BucketSlug"],
            ReadKey = configuration["Cosmic:ReadKey"]
        }
    );
}