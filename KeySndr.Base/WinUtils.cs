using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace KeySndr.Base
{
    public class WinUtils
    {
        public static IntPtr GetWindowHandle(string name)
        {
            var e = Process.GetProcessesByName(name);
            if (e.Any())
                return e.First().MainWindowHandle;
            return (IntPtr)0;
        }

        public static IntPtr FindWindow(string windowName)
        {
            foreach (var p in Process.GetProcesses())
            {
                if (p.MainWindowHandle != IntPtr.Zero && string.Equals(p.MainWindowTitle, windowName, StringComparison.CurrentCultureIgnoreCase))
                    return p.MainWindowHandle;
            }
            return IntPtr.Zero;
        }

        public static IEnumerable<string> GetProcessNames()
        {
            return
                Process.GetProcesses()
                    .Where(p => !string.IsNullOrEmpty(p.MainWindowTitle))
                    .Select(p => p.ProcessName);
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
