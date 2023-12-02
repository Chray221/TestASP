using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TestASP.Model;
using TestASP.Data;
using TestASP.Data.Enums;
using TestASP.Domain.Contexts;
using TestASP.API.Helpers;

namespace SwaggerTest.Controllers
{
    //[SwaggerTag("WeatherForecast")]
    [ApiVersion("1")]
    [ApiController]
    [Route("v{version:apiVersion}/api/[controller]")]
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

        [ProducesResponseType(typeof(IEnumerable<WeatherForecast>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        [HttpGet(Name = "GetWeatherForecast")]
        public IActionResult GetTestGetWeatherForecast(DateOnly? startDate)
        {
            return MessageHelper.Ok(Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = (startDate ?? DateOnly.FromDateTime(DateTime.Now)).AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            }).ToArray(), "Successfully retrieve data.");
        }

        [ApiVersion("2.0")]
        [ProducesResponseType(typeof(IEnumerable<WeatherForecast>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        [HttpGet()]
        public IActionResult GetWeatherForecastV2(DateOnly? startDate)
        {
            return MessageHelper.Ok(Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = (startDate ?? DateOnly.FromDateTime(DateTime.Now)).AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            }).ToArray(), "Successfully retrieve data.");
        }

        [ApiVersion("2")]
        [SwaggerOperation(Summary = "GetWeatherForecast Swagger Annotation", Description = "this is only a sample description")]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(IEnumerable<WeatherForecast>), Description = "Success")]
        [HttpGet("SwaggerAnnot")]
        public IActionResult GetUsingSwaggerAnnot(DateOnly? startDate)
        {
            return MessageHelper.Ok(Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = (startDate ?? DateOnly.FromDateTime(DateTime.Now)).AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            }).ToArray(), "Successfully retrieve data.");
        }
    }
}