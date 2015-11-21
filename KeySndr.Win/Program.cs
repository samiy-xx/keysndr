using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using KeySndr.Base;
using KeySndr.Common.Providers;
using KeySndr.Win.Providers;

namespace KeySndr.Win
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var providers = new List<IProvider>()
            {
                new LoggingProvider()
            };
            var keysndr = new KeySndrApp(providers);
            keysndr.Run();
            Application.Run(new Form1(keysndr));
        }
    }
}
