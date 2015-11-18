using System;
using KeySndr.Common.Providers;

namespace KeySndr.Base.Providers
{
    public interface ILoggingProvider : IProvider
    {
        void Debug(string m);
        void Info(string m);
        void Error(string m, Exception e);
    }
}
