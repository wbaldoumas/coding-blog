using System.Threading.Tasks;
using Coding.Blog.Shared.Services;
using FluentAssertions;
using NUnit.Framework;

namespace Coding.Blog.UnitTests.Services
{
    [TestFixture]
    public class WeatherServiceTests
    {
        [Test]
        public async Task Get_returns_expected_weather_forecasts()
        {
            // arrange
            var weatherService = new WeatherService();

            // act
            var weatherForecasts = await weatherService.Get();

            // assert
            weatherForecasts.Should().NotBeEmpty();
        }
    }
}