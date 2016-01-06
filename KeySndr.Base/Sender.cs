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
    public class SenderInternal
    {
        private readonly InputAction action;
        private const int DefaultDelayAfter = 50;
        public SenderInternal(InputAction a)
        {
            action = a;
        }

        public async Task SendKeyBoardSequences()
        {
            await Task.Run(() =>
            {
                foreach (var sequenceItem in action.Sequences)
                {
                    if (sequenceItem.Entry.Key == null)
                        continue;

                    var keys = new List<Keys>();
                    keys.AddRange(sequenceItem.Modifiers.Select(m => (Keys)m.Value));
                    keys.Add((Keys)sequenceItem.Entry.Value);
                    Keyboard.ShortcutKeys(keys.ToArray(), sequenceItem.KeepDown);
                    Thread.Sleep(sequenceItem.KeepDown + DefaultDelayAfter);
                }
            });
        }

        public async Task SendMouseSequences()
        {
            await Task.Run(() =>
            {
                foreach (var mouseSequenceItem in action.MouseSequences)
                {
                    switch (mouseSequenceItem.Type)
                    {
                        case 0:
                            Mouse.ButtonDown((Mouse.MouseKeys)mouseSequenceItem.Button);
                            Thread.Sleep(mouseSequenceItem.Time);
                            Mouse.ButtonUp((Mouse.MouseKeys)mouseSequenceItem.Button);
                            break;
                        case 1:
                            Mouse.Move(mouseSequenceItem.X, mouseSequenceItem.Y);
                            break;
                        default:
                            Mouse.MoveRelative(mouseSequenceItem.X, mouseSequenceItem.Y);
                            break;
                    }
                    Thread.Sleep(DefaultDelayAfter);
                }
            });
        }

        public async Task SendScripts()
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
                    catch (Exception)
                    {
                        
                    }
                }
            });
        }
    }

    public class Sender
    {
        private static readonly ILoggingProvider Log = ObjectFactory.GetProvider<ILoggingProvider>();

        public async static Task Send(InputActionExecutionContainer container)
        {
            var processSet = false;

            if (container.UseForegroundWindow)
            {
                var ptr = WindowsApi.GetForegroundWindow();
                if (ptr != IntPtr.Zero)
                {
                    WindowsApi.SetFocus(ptr);
                    processSet = true;
                }
            }

            if (!processSet && container.UseDesktop)
            {
                var ptr = WindowsApi.GetDesktopWindow();
                if (ptr != IntPtr.Zero)
                {
                    WindowsApi.SetFocus(ptr);
                    WindowsApi.SetForegroundWindow(ptr);
                    processSet = true;
                }
            }
            if (!processSet && !string.IsNullOrEmpty(container.ProcessName))
            {
                var process = WinUtils.GetProcessByName(container.ProcessName);
                if (process != null)
                {
                    WindowsApi.SetForegroundWindow(process.MainWindowHandle);
                    WindowsApi.SetFocus(process.MainWindowHandle);
                    processSet = true;
                }
                
            }

           // if (!processSet)
            //{
            //    await Send(container.InputAction);
            //    return;
            //}
            
            if (processSet)
                await new Sender().SendAction(container.InputAction);
        }

        public async static Task Send(InputAction a)
        {
            await new Sender().SendAction(a);
        }

        public async Task SendAction(InputAction action)
        {
            var sndr = new SenderInternal(action);
           
            Log.Debug("Running action " + action.Name);
            
            if (action.HasKeySequences)
                await sndr.SendKeyBoardSequences();
            if (action.HasMouseSequences)
                await sndr.SendMouseSequences();
            if (action.HasScriptSequences)
                await sndr.SendScripts();
        }
    }
}
