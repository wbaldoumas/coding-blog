using Autofac;
using Coding.Blog.Shared.Modules;

namespace Coding.Blog.Server.CompositionRoot
{
    public static class ContainerConfigurator
    {
        public static void Configure(ContainerBuilder builder)
        {
            builder.RegisterModule<WeatherModule>();
        }
    }
}