using System;

namespace Aleab.Common.Logging
{
    public class LogEntry
    {
        public LoggingEventType Severity { get; }
        public string Message { get; }
        public Exception Exception { get; }

        public LogEntry(LoggingEventType severity, string message, Exception exception = null)
        {
            if (message == null)
                throw new ArgumentNullException(nameof(message));
            if (message == string.Empty)
                throw new ArgumentException("empty", nameof(message));

            this.Severity = severity;
            this.Message = message;
            this.Exception = exception;
        }
    }
}