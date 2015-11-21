using System;

namespace KeySndr.Win.Events
{
    public class LogEntryEventArgs : EventArgs
    {
        public string Message { get; private set; }
        public LogEntryEventArgs(string m)
            : base()
        {
            Message = m;
        }
    }

    public class InfoLogEntryEventArgs : LogEntryEventArgs
    {
        public InfoLogEntryEventArgs(string m)
            : base(m)
        { 
        }
    }

    public class DebugLogEntryEventArgs : LogEntryEventArgs
    {
        public DebugLogEntryEventArgs(string m)
            : base(m)
        {
        }
    }

    public class ErrorLogEntryEventArgs : LogEntryEventArgs
    {
        public Exception Exception { get; private set; }
        public ErrorLogEntryEventArgs(string m, Exception e)
            : base(m)
        {
            Exception = e;
        }
    }
}
