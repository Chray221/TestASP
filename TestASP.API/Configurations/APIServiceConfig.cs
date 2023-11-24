using System;
using TestASP.API.Services;
using TestASP.Core.IService;

namespace TestASP.API.Configurations
{
	public static class APIServiceConfig
	{
		public static IServiceCollection RegisterAPIRepository(this IServiceCollection services)
        {
            services.AddSingleton<WeatherForecastService>();
            services.AddSingleton<IJwtSerivceManager, JwtSerivceManager>();
            //services.AddSingleton<IFirebaseStorageService, FirebaseStorageService>();
            return services;
        }
    }
}

