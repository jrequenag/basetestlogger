using Microsoft.AspNetCore.Mvc;

namespace SerilogTestLog.Controllers;
[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase {
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(ILogger<WeatherForecastController> logger) {
        _logger = logger;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get() {
        _logger.LogInformation("Requested a Get over GetWeatherForecast");
        try {
            for (int i = 0; i < 100; i++) {
                if (i == 56)
                    throw new Exception("This is out demo exception");
                _logger.LogInformation("The value of i is {LoopCountValue}", i);
            }
        }
        catch (Exception ex) {
            _logger.LogError(ex, "We caugth this exception in the Get Method Call");
        }

        return Enumerable.Range(1, 5).Select(index => new WeatherForecast {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
    }
}
