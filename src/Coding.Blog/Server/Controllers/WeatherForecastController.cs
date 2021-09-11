using Coding.Blog.Shared.Domain;
using Coding.Blog.Shared.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coding.Blog.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IWeatherService _weatherService;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IWeatherService weatherService)
        {
            _logger = logger;
            _weatherService = weatherService;
        }

        [HttpGet]
        public async Task<IEnumerable<WeatherForecast>> Get() => await _weatherService.Get();
    }
}
