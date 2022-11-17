using Coding.Blog.Engine.Configurations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace Coding.Blog.Engine.Extensions;

[ExcludeFromCodeCoverage]
public static class GrpcResilienceConfigurationExtensions
{
    public static IServiceCollection AddGrpcResilienceConfiguration(
        this IServiceCollection serviceCollection,
        IConfiguration configuration
    ) => serviceCollection.AddSingleton(
        new GrpcResilienceConfiguration
        {
            MaxAttempts = int.Parse(configuration["GrpcResilience:MaxAttempts"]!, CultureInfo.InvariantCulture),
            BackoffMultiplier = double.Parse(configuration["GrpcResilience:BackoffMultiplier"]!, CultureInfo.InvariantCulture),
            InitialBackoffMilliseconds = int.Parse(configuration["GrpcResilience:InitialBackoffMilliseconds"]!, CultureInfo.InvariantCulture),
            MaxBackoffMilliseconds = int.Parse(configuration["GrpcResilience:MaxBackoffMilliseconds"]!, CultureInfo.InvariantCulture),
        }
    );
}