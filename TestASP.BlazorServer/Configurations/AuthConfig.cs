using System;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace TestASP.BlazorServer.Configurations
{
	public static class AuthConfig
    {
		public static IServiceCollection RegisterAuthConfig(this IServiceCollection services)
        {
            services.AddDistributedMemoryCache();
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                          .AddCookie(options =>
                          {
                              options.Cookie.HttpOnly = true;
                              options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                              options.LoginPath = "/Authentication/Login";
                              options.AccessDeniedPath = "/Authentication/AccessDenied";
                              options.SlidingExpiration = true;
                          });
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(100);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            return services;
        }

        public static void UseAuthConfig(this WebApplication? app)
        {
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSession();
        }
	}
}

