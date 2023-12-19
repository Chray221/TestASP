using TestASP.Web.Models;
using TestASP.Data;

namespace TestASP.Web.IServices
{
	public interface IWeatherForecastService
    {
        Task<ApiResult<List<WeatherForecast>>> GetAsync(DateOnly? date);
    }
}

