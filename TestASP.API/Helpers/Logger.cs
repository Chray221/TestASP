using System;
using Microsoft.Extensions.Logging;

namespace TestASP.API.Helpers
{
    public static class Logger
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

        public static void LogMessage<T>( this ILogger<T> _logger, string msg, [System.Runtime.CompilerServices.CallerMemberName] string memberName = "")
        {
            msg = $"{DateTime.Now.ToString("HH:mm:ss tt")} [{Title}]-[{memberName}]: {msg}";
#if DEBUG
            _logger.Log(LogLevel.Debug, msg);
#elif RELEASE

#else
            Console.WriteLine(msg);
#endif
        }
    }
}
