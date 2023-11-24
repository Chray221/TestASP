using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TestASP.Data;
using TestASP.Domain.Contexts;

namespace TestASP.Domain.Configurations
{
    public static class DatabaseConfig
    {
        public static void AddDatabaseConfig(this IServiceCollection services)
        {
            services.AddDbContext<TestDbContext>();
        }

        public static void AddIdentityService(this IServiceCollection services)
        {
            //services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddIdentityCore<ApplicationUser>(options =>
                        {
                            options.User.RequireUniqueEmail = true;
                            options.SignIn.RequireConfirmedAccount = true;

                            options.Password.RequireDigit = true;
                            options.Password.RequiredLength = 8;
                            options.Password.RequireLowercase = true;
                            options.Password.RequireUppercase = true;

                            //options.Tokens.

                            options.Lockout.MaxFailedAccessAttempts = 3;
                            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                        })
                    .AddEntityFrameworkStores<TestDbContext>();
                //.AddDefaultUI()
                //.AddTokenProvider();

            //services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
            //    .AddEntityFrameworkStores<TestDbContext>();

            //services.AddIdentityServer()
            //    .AddApiAuthorization<ApplicationUser, TestDbContext>();

            //services.AddAuthentication()
            //    //.AddIdentityServerJwt()
            //    ;

            //services.AddControllersWithViews();
            //services.AddRazorPages();
        }

        public static void AddIdentity(this WebApplication? app)
        {
            if (app.Environment.IsDevelopment())
            {
                //app.UseMigrationsEndPoint();
            }
        }
    }
}
