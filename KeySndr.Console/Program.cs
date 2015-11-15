using KeySndr.Base;
using KeySndr.Console.Providers;

namespace KeySndr.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var keysndr = new KeySndrApp();

            ObjectFactory.AddProvider(new LoggingProvider());
            keysndr.Run();

            
            System.Console.ReadLine();
        }
    }
}
