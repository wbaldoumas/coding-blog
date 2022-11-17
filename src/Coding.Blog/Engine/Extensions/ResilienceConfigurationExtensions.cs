using System.Diagnostics.CodeAnalysis;
using Coding.Blog.Engine.Configurations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Globalization;

namespace Coding.Blog.Engine.Extensions;

[ExcludeFromCodeCoverage]
public static class ResilienceConfigurationExtensions
{
    public static IServiceCollection AddResilienceConfiguration(
        this IServiceCollection serviceCollection,
        IConfiguration configuration
    ) => serviceCollection.AddSingleton(
        new ResilienceConfiguration
        {
            MedianFirstRetryDelayMilliseconds = int.Parse(configuration["Resilience:MedianFirstRetryDelayMilliseconds"]!, CultureInfo.InvariantCulture),
            RetryCount = int.Parse(configuration["Resilience:RetryCount"]!, CultureInfo.InvariantCulture),
            TimeToLiveMilliseconds = int.Parse(configuration["Resilience:TimeToLiveMilliseconds"]!, CultureInfo.InvariantCulture)
        }
    );
}