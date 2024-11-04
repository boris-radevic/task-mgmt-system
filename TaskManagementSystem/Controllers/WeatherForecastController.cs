using Microsoft.AspNetCore.Mvc;
using TaskManagementSystem.Shared;

namespace TaskManagementSystem.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly RabbitMqService _rabbitMqService;
        private readonly IEventPublisher _testProducer;

        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(
            ILogger<WeatherForecastController> logger,
            RabbitMqService rabbitMqService,
            IEventPublisher testProducer)
        {
            _logger = logger;
            _rabbitMqService = rabbitMqService;
            _testProducer = testProducer;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            var weatherForecast = Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();

            //_testProducer.Publish(weatherForecast, "testingKey");

            return weatherForecast;
        }
    }
}