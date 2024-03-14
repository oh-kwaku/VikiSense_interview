using Microsoft.AspNetCore.Mvc;

namespace VikiSense_interview.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        /// <summary>
        /// Get summaries: Just to simulate query string logging
        /// </summary>
        /// <param name="itemsCount"></param>
        /// <returns></returns>

        [HttpGet("summaries")]
        public IEnumerable<string> GetSummaries(int itemsCount=5)
        {

            return Summaries
                .Take(itemsCount)
                .ToList();
        }

        /// <summary>
        /// Just to 
        /// </summary>
        /// <param name="idx"></param>
        /// <returns></returns>
        [HttpGet("summaries/{idx}")]
        public string GetSummary(int idx)
        {

            return Summaries[idx];
                 
        }
    }
}