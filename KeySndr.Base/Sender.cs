using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KeySndr.Base.Providers;
using KeySndr.Common;
using KeySndr.InputManager;

namespace KeySndr.Base
{
    public class SenderInternal
    {
        private readonly InputAction action;

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
                    var converter = new IntKeysConverter();
                    if (sequenceItem.Entry.Key == null)
                        continue;

                    if (sequenceItem.Modifiers.Any())
                    {
                        foreach (var sequenceKeyValuePair in sequenceItem.Modifiers)
                        {
                            Keyboard.KeyPress(converter.ConvertFromInt(sequenceKeyValuePair.Value),
                                sequenceItem.KeepDown);
                        }
                    }
                    Keyboard.KeyPress(converter.ConvertFromInt(sequenceItem.Entry.Value), sequenceItem.KeepDown);
                    Thread.Sleep(sequenceItem.KeepDown + 10);
                }
            });
        }

        public async Task SendMouseSequences()
        {
            await Task.Run(() =>
            {
                foreach (var mouseSequenceItem in action.MouseSequences)
                {
                    Mouse.Move(mouseSequenceItem.X, mouseSequenceItem.Y);
                    Mouse.ButtonDown((Mouse.MouseKeys)mouseSequenceItem.Button);
                    Thread.Sleep(mouseSequenceItem.Time);
                    Mouse.ButtonUp((Mouse.MouseKeys)mouseSequenceItem.Button);
                    Thread.Sleep(50);
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
                    catch (Exception e)
                    {
                        
                    }
                }
            });
        }
    }

    public class Sender
    {
        private static readonly ILoggingProvider Log = ObjectFactory.GetProvider<ILoggingProvider>();

        public async static Task Send(InputAction action)
        {
            var config = ObjectFactory.GetProvider<IAppConfigProvider>().AppConfig;
            if (config.ProcessNumber == IntPtr.Zero)
            {
                var process = WinUtils.GetProcessByName(config.LastProcessName);
                if (process != null)
                    config.ProcessNumber = process.MainWindowHandle;
            }
            if (config.ProcessNumber == IntPtr.Zero)
                return;

            await new Sender().Send(config.ProcessNumber, action, config.UseForegroundWindow);
        }


        public async Task Send(IntPtr currentHandle, InputAction action, bool useFg)
        {
            var sndr = new SenderInternal(action);
           
            Log.Debug("Running action " + action.Name);
            if (!useFg)
            {
                Log.Debug("Setting focus to chosen window");
                WindowsApi.SetForegroundWindow(currentHandle);
                WindowsApi.SetFocus(currentHandle);
            }
            else
            {
                Log.Debug("Setting focus to foreground window");
                var ptr = WindowsApi.GetForegroundWindow();
                if (ptr != IntPtr.Zero)
                    WindowsApi.SetFocus(ptr);
            }
            
            if (action.HasKeySequences)
                await sndr.SendKeyBoardSequences();
            if (action.HasMouseSequences)
                await sndr.SendMouseSequences();
            if (action.HasScriptSequences)
                await sndr.SendScripts();
        }
    }
}
