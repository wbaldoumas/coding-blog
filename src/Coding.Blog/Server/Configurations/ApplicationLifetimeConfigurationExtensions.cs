using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace Coding.Blog.Server.Configurations;

[ExcludeFromCodeCoverage]
public static class ApplicationLifetimeConfigurationExtensions
{
    public static IServiceCollection AddApplicationLifetimeConfiguration(
        this IServiceCollection serviceCollection,
        ConfigurationManager configuration
    ) => serviceCollection.AddSingleton(
        new ApplicationLifetimeConfiguration
        {
            ApplicationStoppingGracePeriodSeconds = int.Parse(configuration["ApplicationLifetime:ApplicationStoppingGracePeriodSeconds"], CultureInfo.InvariantCulture)
        }
    );
}