using System;
using TestASP.BlazorServer.Models;
using TestASP.Data;
using TestASP.Model;

namespace TestASP.BlazorServer.IServices
{
	public interface IWeatherForecastService
    {
        Task<ApiResult<List<WeatherForecast>>> GetAsync(DateOnly? date);
    }
}

