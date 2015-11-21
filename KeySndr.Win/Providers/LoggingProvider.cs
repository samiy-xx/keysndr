using System;
using KeySndr.Base.Providers;
using KeySndr.Win.Events;

namespace KeySndr.Win.Providers
{
    public class LoggingProvider : ILoggingProvider
    {
        public EventHandler<LogEntryEventArgs> OnLogEntry;

        public void Dispose()
        {

        }

        public void Debug(string m)
        {
            OnLogEntry?.Invoke(this, new DebugLogEntryEventArgs(m));
        }

        public void Info(string m)
        {
            OnLogEntry?.Invoke(this, new InfoLogEntryEventArgs(m));
        }

        public void Error(string m, Exception e)
        {
            OnLogEntry?.Invoke(this, new ErrorLogEntryEventArgs(m, e));
        }
    }
}
