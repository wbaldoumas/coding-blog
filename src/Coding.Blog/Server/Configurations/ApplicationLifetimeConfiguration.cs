namespace Coding.Blog.Server.Configurations
{
    internal class ApplicationLifetimeConfiguration
    {
        public int ApplicationStoppingGracePeriodSeconds { get; set; }

        public int ApplicationShutdownTimeoutSeconds { get; set; }
    }
}