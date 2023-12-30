using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Coding.Blog.Library.Configurations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Coding.Blog.Library.Extensions;

[ExcludeFromCodeCoverage]
public static class GrpcConfigurationExtensions
{
    public static IServiceCollection AddGrpcConfiguration(
        this IServiceCollection serviceCollection,
        IConfiguration configuration
    ) => serviceCollection.AddSingleton(
        new GrpcConfiguration
        {
            MaxAttempts = int.Parse(configuration["Grpc:MaxAttempts"]!, CultureInfo.InvariantCulture),
            InitialBackoffMilliseconds = int.Parse(configuration["Grpc:InitialBackoffMilliseconds"]!, CultureInfo.InvariantCulture),
            MaxBackoffMilliseconds = int.Parse(configuration["Grpc:MaxBackoffMilliseconds"]!, CultureInfo.InvariantCulture),
            BackoffMultiplier = int.Parse(configuration["Grpc:BackoffMultiplier"]!, CultureInfo.InvariantCulture)
        }
    );
}
