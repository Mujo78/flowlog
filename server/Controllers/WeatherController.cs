using Microsoft.AspNetCore.Mvc;
using server.Models;

namespace server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WeatherController : ControllerBase
    {
        private static readonly string[] Summaries =
        [
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        ];

        public WeatherController() {
            
        }
    
        [HttpGet(Name ="GetWeatherForecast")]
        public IEnumerable<WeatherForecast> GetWeathers() {
          return Enumerable.Range(1, 5).Select(index => new WeatherForecast {
            Id = index,
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
          }).ToArray();  
        }
    }
}
