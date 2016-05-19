using System;
using System.Threading.Tasks;
using KeySndr.Common;

namespace KeySndr.Base
{
    public class ActionProcessor : IActionProcessor
    {
        //private bool hasProcess;
        public InputActionExecutionContainer Container { get; set; }

        public ActionProcessor()
        {
            //hasProcess = false;
        }

        public ActionProcessor(InputActionExecutionContainer c)
            : this()
        {
            Container = c;
        }

        public async Task Process()
        {
            //if (!hasProcess)
            //    SetTargetProcess();
            await Sender.Send(Container);
        }

        /*private void SetTargetProcess()
        {
            if (Container.UseForegroundWindow)
            {
                var ptr = WindowsApi.GetForegroundWindow();
                if (ptr != IntPtr.Zero)
                {
                    WindowsApi.SetFocus(ptr);
                    Sender.SetCurrentProcessTarget(ptr);
                    hasProcess = true;
                }
            }

            if (!hasProcess && Container.UseDesktop)
            {
                var ptr = WindowsApi.GetDesktopWindow();
                if (ptr != IntPtr.Zero)
                {
                    WindowsApi.SetForegroundWindow(ptr);
                    WindowsApi.SetFocus(ptr);
                    Sender.SetCurrentProcessTarget(ptr);
                    hasProcess = true;
                }
            }

            if (hasProcess || string.IsNullOrEmpty(Container.ProcessName))
                return;
            var process = WinUtils.GetProcessByName(Container.ProcessName);
            if (process == null)
                return;
            
            Sender.SetCurrentProcessTarget(process.MainWindowHandle);
            WindowsApi.SetForegroundWindow(process.MainWindowHandle);
            WindowsApi.SetFocus(process.MainWindowHandle);
            hasProcess = true;
        }*/
    }
}
