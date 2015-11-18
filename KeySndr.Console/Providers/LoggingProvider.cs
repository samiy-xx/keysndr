using System;
using KeySndr.Base.Providers;

namespace KeySndr.Console.Providers
{
    public class LoggingProvider : ILoggingProvider
    {
        public void Debug(string m)
        {
            System.Console.WriteLine(m);
        }

        public void Info(string m)
        {
            System.Console.WriteLine(m);
        }

        public void Error(string m, Exception e)
        {
            System.Console.WriteLine(m);
        }

        public void Dispose()
        {
           
        }
    }
}
