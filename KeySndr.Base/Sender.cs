using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KeySndr.Base.Providers;
using KeySndr.Common;
using MacroToolServer.Input;

namespace KeySndr.Base
{
    public class Sender
    {
        private static readonly ILoggingProvider Log = ObjectFactory.GetProvider<ILoggingProvider>();

        public async static Task Send(InputAction action)
        {
            var config = ObjectFactory.GetProvider<IAppConfigProvider>().AppConfig;
            await Send(config.ProcessNumber, action, config.UseForegroundWindow);
        }

        public static Task Send(IntPtr currentHandle, InputAction action, bool useFg)
        {
            return Task.Run(() =>
            {
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

                Log.Debug("Running keyboard sequences for action " + action.Name);
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

                Log.Debug("Running Mouse sequences for action " + action.Name);
                foreach (var mouseSequenceItem in action.MouseSequences)
                {
                    Mouse.Move(mouseSequenceItem.X, mouseSequenceItem.Y);
                    Mouse.ButtonDown((Mouse.MouseKeys)mouseSequenceItem.Button);
                    Thread.Sleep(mouseSequenceItem.Time);
                    Mouse.ButtonUp((Mouse.MouseKeys)mouseSequenceItem.Button);
                    Thread.Sleep(50);
                }

                Log.Debug("Running scripts for action " + action.Name);
                foreach (var scriptSequence in action.ScriptSequences)
                {
                    Log.Debug("Attempting to find scripting context for " + scriptSequence.ScriptName);
                    var ctx = ObjectFactory.GetProvider<IScriptProvider>().FindContextForName(scriptSequence.ScriptName);

                    if (!ctx.IsValid || !ctx.HasBeenExecuted)
                    {
                        Log.Debug("Scripting context for " + scriptSequence.ScriptName + " was not valid, Run tests.");
                        continue;
                    }

                    try
                    {
                        Log.Debug("Running script " + scriptSequence.ScriptName);
                        ctx.Run();
                    }
                    catch (Exception e)
                    {
                        Log.Error("Error in running script " + scriptSequence.ScriptName, e);
                    }
                }
                return true;
            });
        }
    }
}
