using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using KeySndr.Base.Providers;
using KeySndr.Common;
using KeySndr.InputManager;

namespace KeySndr.Base
{
    public static class Sender
    {
        private static readonly ILoggingProvider Log = ObjectFactory.GetProvider<ILoggingProvider>();
        private const int DefaultDelayAfter = 50;
        private static IntPtr currentIntPtr = IntPtr.Zero;

        public static async Task Send(InputActionExecutionContainer container)
        {   
            
            await Send(container, container.InputAction);
        }

        public static async Task Send(InputAction action)
        {
            await Send(null, action);
        }

        public static async Task Send(InputActionExecutionContainer container, InputAction action)
        {
            Log.Debug("Running action " + action.Name);
            if (action.OverrideProcess && !string.IsNullOrEmpty(action.ProcessName))
                SetTargetProcess(action);
            else if (container != null)
                SetTargetProcess(container);

            if (action.HasKeySequences)
                await SendKeyBoardSequences(action);
            if (action.HasMouseSequences)
                await SendMouseSequences(action);
            if (action.HasScriptSequences)
                await SendScripts(action);

            //if (container != null)
            //    SetTargetProcess(container);
        }

        public static void SetCurrentProcessTarget(IntPtr p)
        {
            currentIntPtr = p;
        }

        public static IntPtr GetCurrentProcessTarget()
        {
            return currentIntPtr;
        }

        public async static Task Send(MouseSequenceItem item)
        {
            ExecuteMouseSequenceItem(item);
        }

        public async static Task Send(SequenceItem item)
        {
            ExecuteSequenceItem(item);
        }

        public async static Task Send(SequenceItem[] items)
        {
            ExecuteSequenceItems(items);
        }

        public static async Task SendKeyBoardSequences(InputAction action)
        {
            await Task.Run(() =>
            {
                foreach (var sequenceItem in action.Sequences)
                {
                    if (sequenceItem.Entry.Key == null)
                        continue;

                    ExecuteSequenceItem(sequenceItem);
                }
            });
        }

        public static async Task SendMouseSequences(InputAction action)
        {
            await Task.Run(() =>
            {
                foreach (var mouseSequenceItem in action.MouseSequences)
                {
                    ExecuteMouseSequenceItem(mouseSequenceItem);
                    Thread.Sleep(mouseSequenceItem.Time);
                }
            });
        }

        public static async Task SendScripts(InputAction action)
        {
            await Task.Run(() =>
            {
                foreach (var scriptSequence in action.ScriptSequences)
                {
                    var ctx = ObjectFactory.GetProvider<IScriptProvider>().FindContextForName(scriptSequence.ScriptName);
                    if (!ctx.IsValid || !ctx.HasBeenExecuted)
                        continue;

                    try
                    {
                        ctx.Run();
                    }
                    catch (Exception e)
                    {
                        ObjectFactory.GetProvider<ILoggingProvider>().Error(e.Message, e);
                    }
                }
            });
        }

        public static void ExecuteSequenceItem(SequenceItem item)
        {
            var keys = new List<Keys>();
            keys.AddRange(item.Modifiers.Select(m => (Keys)m.Value));
            keys.Add((Keys)item.Entry.Value);
            ExecuteKeyboardCommand(keys, item.KeepDown);
            //if (item.Method.Equals("sendinput"))
            //    ExecuteKeyboardCommand(keys, item.KeepDown);
            //else
            //    ExecuteVirtualKeyboardCommand(keys, item.KeepDown);
            Thread.Sleep(item.KeepDown);
        }

        public static void ExecuteKeyboardCommand(IEnumerable<Keys> keys, int keepDown)
        {
            //VirtualKeyboard.ShortcutKeys(keys.ToArray(), keepDown);
            Keyboard.ShortcutKeys(keys.ToArray(), keepDown);
        }

        public static void ExecuteVirtualKeyboardCommand(IEnumerable<Keys> keys, int keepDownm)
        {   
            VirtualKeyboard.ShortcutKeys(keys.ToArray(), keepDownm);
        }

        public static void ExecuteSequenceItems(SequenceItem[] items)
        {
            foreach (var sequenceItem in items)
            {
                ExecuteSequenceItem(sequenceItem);
            }
        }

        public static void ExecuteMouseSequenceItem(MouseSequenceItem item)
        {
            switch (item.Type)
            {
                case 0:
                    Mouse.ButtonDown((Mouse.MouseKeys)item.Button);
                    Thread.Sleep(item.Time);
                    Mouse.ButtonUp((Mouse.MouseKeys)item.Button);
                    break;
                case 1:
                    Mouse.Move(item.X, item.Y);
                    break;
                default:
                    Mouse.MoveRelative(item.X, item.Y);
                    break;
            }
        }

        private static void SetTargetProcess(InputActionExecutionContainer container)
        {
            var hasProcess = false;
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
        }

        private static void SetTargetProcess(InputAction action)
        {
            var process = WinUtils.GetProcessByName(action.ProcessName);
            if (process == null)
                return;

            Sender.SetCurrentProcessTarget(process.MainWindowHandle);
            if (action.BringProcessToForeground)
                WindowsApi.SetForegroundWindow(process.MainWindowHandle);
            WindowsApi.SetFocus(process.MainWindowHandle);
        }

        public static void SetTargetProcess(string name, bool f)
        {
            var process = WinUtils.GetProcessByName(name);
            if (process == null)
                return;

            Sender.SetCurrentProcessTarget(process.MainWindowHandle);
            if (f)
                WindowsApi.SetForegroundWindow(process.MainWindowHandle);
            WindowsApi.SetFocus(process.MainWindowHandle);
        }
        /*private static void SetTargetProcess()
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
        }
        /
        */
    }
}
