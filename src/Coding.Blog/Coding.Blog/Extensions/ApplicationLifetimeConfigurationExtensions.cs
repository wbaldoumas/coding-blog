using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Coding.Blog.Library.Configurations;

namespace Coding.Blog.Extensions;

[ExcludeFromCodeCoverage]
public static class ApplicationLifetimeConfigurationExtensions
{
    public static IServiceCollection AddApplicationLifetimeConfiguration(
        this IServiceCollection serviceCollection,
        ConfigurationManager configuration
    ) => serviceCollection.AddSingleton(
        new ApplicationLifetimeConfiguration
        {
            ApplicationStoppingGracePeriodSeconds = int.Parse(configuration["ApplicationLifetime:ApplicationStoppingGracePeriodSeconds"]!, CultureInfo.InvariantCulture)
        }
    );
}
