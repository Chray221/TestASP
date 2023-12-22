using System;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace TestASP.Web.Configurations
{
	public static class AuthConfig
    {
		public static IServiceCollection RegisterAuthConfig(this IServiceCollection services)
        {
            services.AddMemoryCache();
            // services.AddDistributedMemoryCache();
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                    .AddCookie(options =>
                    {
                    
                        //options.Cookie.Domain = // do I need to set this?
                        // options.Cookie.SecurePolicy = _environment.IsDevelopment() ? CookieSecurePolicy.None : CookieSecurePolicy.Always;
                        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                        options.Cookie.SameSite = SameSiteMode.Strict;
                        options.Cookie.Name = "TestASP.Web.Cookie";
                        options.Cookie.HttpOnly = true;
                        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                        options.LoginPath = "/Authentication/Login";
                        options.AccessDeniedPath = "/Authentication/Login";
                        options.LogoutPath = "/Authentication/Logout";
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
            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSession();
        }
	}
}

