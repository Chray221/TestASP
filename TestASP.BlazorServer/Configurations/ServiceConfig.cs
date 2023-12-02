using System;
using TestASP.BlazorServer.Extensions;
using TestASP.BlazorServer.IServices;
using TestASP.BlazorServer.Services;

namespace TestASP.BlazorServer.Configurations
{
	public static class ServiceConfig
	{
		public static IServiceCollection RegisterServices(this IServiceCollection services)
		{
            //services.AddHttpClient<IVillaService, VillaService>();
            //services.AddScoped<IVillaService, VillaService>();
            //services.AddHttpClient<IVillaNumberService, VillaNumberService>();
            //services.AddScoped<IVillaNumberService, VillaNumberService>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddApiService<IAuthService, AuthService>();
            services.AddApiService<IWeatherForecastService, WeatherForecastService>();

            //services
            //services.AddTransient<IAuthService, AuthService>();

            return services;
		}
	}
}

