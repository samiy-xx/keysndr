using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeySndr.Base
{
    public class WinUtils
    {
        public static IntPtr GetWindowHandle(string name)
        {
            var e = System.Diagnostics.Process.GetProcessesByName(name);
            if (e.Any())
                return e.First().MainWindowHandle;
            return (IntPtr)0;
        }

        public static IntPtr FindWindow(string windowName)
        {
            foreach (System.Diagnostics.Process p in System.Diagnostics.Process.GetProcesses())
            {
                if (p.MainWindowHandle != IntPtr.Zero && p.MainWindowTitle.ToLower() == windowName.ToLower())
                    return p.MainWindowHandle;
            }
            return IntPtr.Zero;
        }

        public static IEnumerable<Process> GetProcessesWithWindow()
        {
            return Process.GetProcesses()
                .Where(p => !string.IsNullOrEmpty(p.MainWindowTitle));
        }

        public static Process GetProcessByName(string name)
        {
            return Process.GetProcesses().FirstOrDefault(p => p.ProcessName == name);
        }
    }
}
