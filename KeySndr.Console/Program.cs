using System.Collections.Generic;
using KeySndr.Base;
using KeySndr.Common.Providers;
using KeySndr.Console.Providers;

namespace KeySndr.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var providers = new List<IProvider>();
            providers.Add(new LoggingProvider());

            var keysndr = new KeySndrApp(providers);
            keysndr.Run();

            System.Console.ReadLine();
        }
    }
}
