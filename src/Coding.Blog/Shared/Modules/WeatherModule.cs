using Autofac;
using Coding.Blog.Shared.Services;

namespace Coding.Blog.Shared.Modules
{
    public class WeatherModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<WeatherService>().As<IWeatherService>().SingleInstance();
        }
    }
}