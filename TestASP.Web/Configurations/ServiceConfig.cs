using System;
using TestASP.Web.Extensions;
using TestASP.Web.IServices;
using TestASP.Web.Services;

namespace TestASP.Web.Configurations;

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
            services.AddApiService<IUserService, UserService>();
            services.AddApiService<IQuestionnaireService, QuestionnaireService>();

            //services
            //services.AddTransient<IAuthService, AuthService>();

            return services;
      }
}


