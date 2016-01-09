using System;
using System.Threading.Tasks;
using KeySndr.Common;

namespace KeySndr.Base
{
    public class ActionProcessor : IActionProcessor
    {
        private bool hasProcess;
        private readonly InputActionExecutionContainer container;

        public ActionProcessor(InputActionExecutionContainer c)
        {
            hasProcess = false;
            container = c;
        }

        public async Task Process()
        {
            if (!hasProcess)
                SetTargetProcess();
            await Sender.Send(container);
        }

        private void SetTargetProcess()
        {
            if (container.UseForegroundWindow)
            {
                var ptr = WindowsApi.GetForegroundWindow();
                if (ptr != IntPtr.Zero)
                {
                    WindowsApi.SetFocus(ptr);
                    Sender.SetCurrentProcessTarget(ptr);
                    hasProcess = true;
                }
            }

            if (!hasProcess && container.UseDesktop)
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

            if (hasProcess || string.IsNullOrEmpty(container.ProcessName))
                return;
            var process = WinUtils.GetProcessByName(container.ProcessName);
            if (process == null)
                return;
            
            Sender.SetCurrentProcessTarget(process.MainWindowHandle);
            WindowsApi.SetForegroundWindow(process.MainWindowHandle);
            WindowsApi.SetFocus(process.MainWindowHandle);
            hasProcess = true;
        }
    }
}
