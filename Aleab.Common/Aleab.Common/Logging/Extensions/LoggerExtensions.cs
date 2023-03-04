using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Aleab.Common.Logging.Interfaces;

namespace Aleab.Common.Logging.Extensions
{
    public static class LoggerExtensions
    {
        #region Static members

        public static void Debug(this ILogger logger, string message, Exception exception = null,
            bool includeCallerClass = false,
            [CallerFilePath] string callerFilePath = null,
            [CallerMemberName] string callerMemberName = null,
            [CallerLineNumber] int callerLineNumber = -1)
        {
            Log(logger, new LogEntry(LoggingEventType.Debug, message, exception), callerFilePath, callerMemberName, callerLineNumber, includeCallerClass);
        }

        public static void Info(this ILogger logger, string message, Exception exception = null,
            bool includeCallerClass = false,
            [CallerFilePath] string callerFilePath = null,
            [CallerMemberName] string callerMemberName = null,
            [CallerLineNumber] int callerLineNumber = -1)
        {
            Log(logger, new LogEntry(LoggingEventType.Information, message, exception), callerFilePath, callerMemberName, callerLineNumber, includeCallerClass);
        }

        public static void Warn(this ILogger logger, string message, Exception exception = null,
            bool includeCallerClass = true,
            [CallerFilePath] string callerFilePath = null,
            [CallerMemberName] string callerMemberName = null,
            [CallerLineNumber] int callerLineNumber = -1)
        {
            Log(logger, new LogEntry(LoggingEventType.Warning, message, exception), callerFilePath, callerMemberName, callerLineNumber, includeCallerClass);
        }

        public static void Error(this ILogger logger, string message, Exception exception = null,
            bool includeCallerClass = true,
            [CallerFilePath] string callerFilePath = null,
            [CallerMemberName] string callerMemberName = null,
            [CallerLineNumber] int callerLineNumber = -1)
        {
            Log(logger, new LogEntry(LoggingEventType.Error, message, exception), callerFilePath, callerMemberName, callerLineNumber, includeCallerClass);
        }

        public static void Fatal(this ILogger logger, string message, Exception exception = null,
            bool includeCallerClass = true,
            [CallerFilePath] string callerFilePath = null,
            [CallerMemberName] string callerMemberName = null,
            [CallerLineNumber] int callerLineNumber = -1)
        {
            Log(logger, new LogEntry(LoggingEventType.Fatal, message, exception), callerFilePath, callerMemberName, callerLineNumber, includeCallerClass);
        }

        private static void Log(ILogger logger, LogEntry logEntry, string callerFilePath, string callerMemberName, int callerLineNumber, bool includeCallerClass)
        {
            string callerClassName = includeCallerClass ? new StackTrace().GetFrame(2)?.GetMethod()?.DeclaringType?.FullName : null;
            logger.Log(logEntry, callerFilePath, callerClassName, callerMemberName, callerLineNumber);
        }

        #endregion
    }
}