using System.Collections.Generic;
using System.Threading.Tasks;
using Coding.Blog.Shared.Domain;

namespace Coding.Blog.Shared.Services
{
    public interface IWeatherService
    {
        Task<IEnumerable<WeatherForecast>> Get();
    }
}