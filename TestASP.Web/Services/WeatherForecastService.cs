using TestASP.Web.IServices;
using TestASP.Web.Models;
using TestASP.Common.Utilities;
using TestASP.Data;

namespace TestASP.Web.Services
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
                return SendAsync<object, List<WeatherForecast>>(ApiRequest.GetRequest($"{ApiEndpoints.WeatherForecastUrl}?startDate={date}"));
            }
            return SendAsync<object,List < WeatherForecast >> (ApiRequest.GetRequest(ApiEndpoints.WeatherForecastUrl));
        }
    }
}

