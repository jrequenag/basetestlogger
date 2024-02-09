using Microsoft.AspNetCore.Mvc;

using System.Globalization;

namespace SerilogTestLog.Controllers;
[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase {
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;
    private readonly IServiceBus _serviceBus;
    private readonly ApplicationContext _applicationContext;

    public WeatherForecastController(ILogger<WeatherForecastController> logger, IServiceBus serviceBus, ApplicationContext applicationContext) {
        _logger = logger;
        _serviceBus = serviceBus;
        _applicationContext = applicationContext;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public async Task<IEnumerable<WeatherForecast>> Get() {

        CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
        CultureInfo.CurrentUICulture = CultureInfo.InvariantCulture;
        _logger.LogInformation("Requested a Get over GetWeatherForecast");
        try {
            throw new Exception("Este error es personalizado, para observar que se registra");
            foreach (Customer? item in _applicationContext.Customer.ToList()) {
                _logger.LogInformation("[Test] Found person with id {LoopCountValue}", item.CustomerId);
                _ = _serviceBus.SendMessageAsync(item.CustomerId.ToString());
            }
            //for (int i = 0; i < 100; i++) {
            //    //if (i == 56)
            //    //    throw new Exception("This is out demo exception");
            //    //
            //    var customer = _applicationContext.Customer.Fir

            //}
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
//testslogger
//wiouf98w79234fuiojf@2