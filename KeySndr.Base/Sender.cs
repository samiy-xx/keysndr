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
            await Send(container.InputAction);
        }

        public static async Task Send(InputAction action)
        {
            Log.Debug("Running action " + action.Name);

            if (action.HasKeySequences)
                await SendKeyBoardSequences(action);
            if (action.HasMouseSequences)
                await SendMouseSequences(action);
            if (action.HasScriptSequences)
                await SendScripts(action);
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
            Keyboard.ShortcutKeys(keys.ToArray(), item.KeepDown);
            Thread.Sleep(item.KeepDown);
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

        /*public static async Task SendAction(InputAction action)
        {
            var sndr = new SenderInternal();
           
            Log.Debug("Running action " + action.Name);
            
            if (action.HasKeySequences)
                await sndr.SendKeyBoardSequences(action);
            if (action.HasMouseSequences)
                await sndr.SendMouseSequences(action);
            if (action.HasScriptSequences)
                await sndr.SendScripts(action);
        }*/
    }
}
