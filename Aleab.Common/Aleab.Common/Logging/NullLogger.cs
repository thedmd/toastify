using Aleab.Common.Logging.Interfaces;

namespace Aleab.Common.Logging
{
    internal class NullLogger : ILogger
    {
        public void Log(LogEntry entry)
        {
        }

        public void Log(LogEntry entry, bool includeCallerClass, string callerFilePath = null, string callerMemberName = null, int callerLineNumber = -1)
        {
        }

        public void Log(LogEntry entry, string callerFilePath, string callerClassName, string callerMemberName, int callerLineNumber)
        {
        }
    }
}