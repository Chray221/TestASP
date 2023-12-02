using System;
using System.Diagnostics.CodeAnalysis;

namespace TestASP.BlazorServer.Extensions
{
	public static class LoggerExtension
    {
        public static string Title = "TestAPI";
        public static void Log(string msg, [System.Runtime.CompilerServices.CallerMemberName] string memberName = "")
        {
            msg = $"{DateTime.Now.ToString("HH:mm:ss tt")} [{Title}]-[{memberName}]: {msg}";
#if DEBUG
            System.Diagnostics.Debug.WriteLine(msg);
            Console.WriteLine(msg);
#elif RELEASE

#else
            Console.WriteLine(msg);
#endif
        }

        public static void LogMessage<T>(this ILogger<T> _logger, string msg, [System.Runtime.CompilerServices.CallerMemberName] string memberName = "")
        {
            msg = $"{DateTime.Now.ToString("HH:mm:ss tt")} [{Title}]-[{memberName}]: {msg}";
#if DEBUG
            _logger.Log(LogLevel.Information, msg);
#elif RELEASE

#else
            Console.WriteLine(msg);
#endif
        }

        public static void AddApiService<IService, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Service>(this IServiceCollection services)
            where IService: class
            where Service: class, IService
        {
            services.AddHttpClient<IService, Service>();
            services.AddScoped<IService, Service>();
        }
    }
}

