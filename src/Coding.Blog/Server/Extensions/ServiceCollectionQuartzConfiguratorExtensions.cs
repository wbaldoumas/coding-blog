using Quartz;
using System.Configuration;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace Coding.Blog.Server.Extensions;

[ExcludeFromCodeCoverage]
internal static class ServiceCollectionQuartzConfiguratorExtensions
{
    /// <summary>
    ///     Adds a job and trigger to the <paramref name="serviceCollectionQuartzConfigurator"/>.
    /// </summary>
    /// <typeparam name="T"> The type of the job. </typeparam>
    /// <param name="serviceCollectionQuartzConfigurator"> The service collection quartz configurator. </param>
    /// <param name="configuration"> The configuration. </param>
    /// <exception cref="ConfigurationErrorsException"> Thrown when the job's configuration is missing or invalid. </exception>
    public static IServiceCollectionQuartzConfigurator ConfigureJob<T>(
        this IServiceCollectionQuartzConfigurator serviceCollectionQuartzConfigurator,
        IConfiguration configuration)
        where T : IJob
    {
        var jobName = typeof(T).Name;
        var intervalSeconds = GetIntervalSeconds(jobName, configuration);
        var jobKey = new JobKey(jobName);

        serviceCollectionQuartzConfigurator.AddJob<T>(jobKey);

        return serviceCollectionQuartzConfigurator.AddTrigger(triggerConfigurator => triggerConfigurator
            .ForJob(jobKey)
            .WithSimpleSchedule(simpleScheduleBuilder => simpleScheduleBuilder
                .WithIntervalInSeconds(intervalSeconds)
                .RepeatForever()
            )
            .StartNow()
        );
    }

    private static int GetIntervalSeconds(string jobName, IConfiguration configuration)
    {
        var configurationKey = $"Quartz:{jobName}:IntervalSeconds";
        var intervalSecondsConfiguration = configuration[configurationKey];

        if (string.IsNullOrEmpty(intervalSecondsConfiguration))
        {
            throw new ConfigurationErrorsException($"No job interval configuration found for {configurationKey}.");
        }

        if (!int.TryParse(intervalSecondsConfiguration, NumberStyles.Integer, CultureInfo.InvariantCulture, out var intervalSeconds) || intervalSeconds <= 0)
        {
            throw new ConfigurationErrorsException(
                $"Invalid job interval configuration found for {configurationKey}. Job interval configuration: {intervalSecondsConfiguration}."
            );
        }

        return intervalSeconds;
    }
}