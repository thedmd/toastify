using System.Runtime.CompilerServices;

namespace Aleab.Common.Logging.Interfaces
{
    public interface ILogger
    {
        void Log(LogEntry entry);
        void Log(LogEntry entry, bool includeCallerClass, [CallerFilePath] string callerFilePath = null, [CallerMemberName] string callerMemberName = null, [CallerLineNumber] int callerLineNumber = -1);
        void Log(LogEntry entry, string callerFilePath, string callerClassName, string callerMemberName, int callerLineNumber);
    }
}