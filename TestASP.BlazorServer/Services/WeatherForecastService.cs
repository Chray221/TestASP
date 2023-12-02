using System;
using System.Collections.Generic;
using TestASP.BlazorServer.IServices;
using TestASP.BlazorServer.Models;
using TestASP.Common.Utilities;
using TestASP.Data;

namespace TestASP.BlazorServer.Services
{
	public class WeatherForecastService : BaseApiService, IWeatherForecastService
	{

        public WeatherForecastService(
            IHttpClientFactory httpClient,
            ILogger<WeatherForecastService> logger,
            ConfigurationManager configuration)
            : base(httpClient, logger, configuration)
        {
        }

        public Task<ApiResult<List<WeatherForecast>>> GetAsync(DateOnly? date)
        {
            if(date != null)
            {
                return SendAsync<object, List<WeatherForecast>>(ApiRequest.GetRequest($"{ApiEndpoints.WeatherForecast}?startDate={date}"));
            }
            return SendAsync<object,List < WeatherForecast >> (ApiRequest.GetRequest(ApiEndpoints.WeatherForecast));
        }
    }
}

