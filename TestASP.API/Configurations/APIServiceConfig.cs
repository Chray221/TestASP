using System;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using System.Text;
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

        public static IServiceCollection RegisterAuthentication(this IServiceCollection services, ConfigurationManager Configuration)
        {
            services.AddAuthentication(options =>
            {
                //options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                #region For JWT Auth
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                #endregion

                #region For JWT & Cookie Authentication
                //options.DefaultAuthenticateScheme = "JWT_OR_COOKIE";
                //options.DefaultChallengeScheme = "JWT_OR_COOKIE";
                //options.DefaultScheme = "JWT_OR_COOKIE";
                #endregion
            })
            // Adding Jwt Bearer  
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = Configuration["JWT:ValidAudience"],
                    ValidIssuer = Configuration["JWT:ValidIssuer"],
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:Secret"]))
                };
            })
            // add cookie for cookie auth
            /*
             .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
            {
                options.ExpireTimeSpan = TimeSpan.FromDays(1);
                options.LoginPath = "/Authentication/Signin";
            })
            // this is the key piece!
            .AddPolicyScheme("JWT_OR_COOKIE", "JWT_OR_COOKIE", options =>
            {
                // runs on each request
                options.ForwardDefaultSelector = context =>
                {
                    // filter by auth type
                    string authorization = context.Request.Headers[HeaderNames.Authorization];
                    if (!string.IsNullOrEmpty(authorization) && authorization.StartsWith("Bearer "))
                        return JwtBearerDefaults.AuthenticationScheme;

                    // otherwise always check for cookie auth
                    return CookieAuthenticationDefaults.AuthenticationScheme;
                };
            })
             */;
            services.AddAuthorization();

            return services;
        }
    }

}

