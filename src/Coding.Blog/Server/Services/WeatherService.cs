using Coding.Blog.Shared;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;

namespace Coding.Blog.Server.Services;

public class WeatherService : WeatherForecasts.WeatherForecastsBase
{
    private static readonly string[] Summaries =
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    public override Task<WeatherReply> GetWeather(WeatherForecast request, ServerCallContext context)
    {
        var reply = new WeatherReply();
        var rng = new Random();

        reply.Forecasts.Add(Enumerable.Range(1, 10).Select(_ => new WeatherForecast
        {
            Date = Timestamp.FromDateTime(DateTime.UtcNow),
            TemperatureC = rng.Next(20, 55),
            Summary = Summaries[rng.Next(Summaries.Length)]
        }));

        return Task.FromResult(reply);
    }
}