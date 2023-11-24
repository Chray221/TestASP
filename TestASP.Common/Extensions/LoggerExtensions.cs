using System;
using Microsoft.Extensions.Logging;

namespace TestASP.Common.Extensions
{
	public static class LoggerExtensions
	{
        static string Title = "TestASP";
        private static void Log(string msg, [System.Runtime.CompilerServices.CallerMemberName] string memberName = "")
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

        public static void LogMessage(this ILogger _logger, string msg, [System.Runtime.CompilerServices.CallerMemberName] string memberName = "")
        {
#if DEBUG
            _logger.Log(LogLevel.Debug,
                        "{DateTime} [{Title}]-[{MemberName}]: {LogMessage}",
                        DateTime.Now.ToString("HH:mm:ss tt"),
                        Title,
                        memberName,
                        msg);
#elif RELEASE

#else
            msg = $"{DateTime.Now.ToString("HH:mm:ss tt")} [{Title}]-[{memberName}]: {msg}";
            Console.WriteLine(msg);
#endif
        }

        public static void LogFormat(this ILogger _logger, string format, params string[] msgs)
        {

#if DEBUG
            _logger.Log(LogLevel.Debug,
                        "{DateTime} [{Title}]: "+ format,
                        DateTime.Now.ToString("HH:mm:ss tt"),
                        Title,
                        msgs);
#elif RELEASE

#else
            msg = $"{DateTime.Now.ToString("HH:mm:ss tt")} [{Title}]-[{memberName}]: {msg}";
            Console.WriteLine(msg);
#endif
        }

        public static void LogException(this ILogger logger, Exception exception)
        {
            logger.LogInformation("[{DateTime}] MESSAGE: {Message} \nSTACKTRACE: {StackTrace}",
                                  DateTime.Now.ToString("HH:mm:ss tt"),
                                  exception.Message,
                                  exception.StackTrace);
        }
    }
}

