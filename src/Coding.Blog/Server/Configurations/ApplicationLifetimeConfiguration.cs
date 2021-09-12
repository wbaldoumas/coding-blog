namespace Coding.Blog.Server.Configurations
{
    /// <summary>
    ///     Controls how long the application should stay alive after receiving a SIGTERM
    ///     signal. This prevents in-flight traffic from being dropped during deployments or
    ///     application restarts.
    /// </summary>
    internal class ApplicationLifetimeConfiguration
    {
        public static string Key => "ApplicationLifetime";

        public int ApplicationStoppingGracePeriodSeconds { get; set; }

        public int ApplicationShutdownTimeoutSeconds { get; set; }
    }
}