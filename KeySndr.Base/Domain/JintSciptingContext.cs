using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Threading;
using System.Windows.Forms;
using Jint;
using Jint.Native;
using Jint.Runtime;
using KeySndr.Base.Exceptions;
using KeySndr.Base.Providers;
using KeySndr.Common;

namespace KeySndr.Base.Domain
{
    public class JintScriptingContext : IScriptContext
    {
        private Engine engine;
        public bool HasBeenParsed { get; private set; }
        public bool IsValid { get; private set; }
        public bool IsRunning { get; set; }
        public bool HasBeenExecuted { get; private set; }
        public InputScript Script { get; set; }
        private bool testMode = false;

        public JintScriptingContext()
        {
            IsValid = false;
            HasBeenParsed = false;
            IsRunning = false;
            HasBeenExecuted = false;

            engine = new Engine(cfg => cfg
                .AllowClr()
                .AllowClr(typeof(InputAction).Assembly)
                .AllowClr(typeof(JintScriptingContext).Assembly)
            );
            
        }

        public JintScriptingContext(InputScript script)
            : this()
        {
            Script = script;
            Script.Context = this;
            Expose();
        }

        public void SetTestMode(bool b)
        {
            testMode = b;
        }

        public void Execute()
        {

            foreach (var sourceFile in Script.SourceFiles)
            {
                try
                {
                    engine.Execute(sourceFile.Contents);
                    HasBeenExecuted = true;
                    IsValid = true;
                    HasBeenParsed = true;
                    SetSourceValid(sourceFile);
                }
                catch (Exception e)
                {
                    SetSourceInValid(sourceFile, e);
                    HasBeenParsed = false;
                    IsValid = false;
                    HasBeenParsed = false;
                    throw;
                }
            }

        }

        public void Run()
        {
            ObjectFactory.GetProvider<ILoggingProvider>().Info("Running script "+ Script.Name);
            if (!HasBeenExecuted)
                throw new ScriptException("Script has not been executed");

            IsRunning = true;
            try
            {
                engine.Invoke("execute").ToObject();
                Script.IsValid = true;
                Script.Errors.Clear();
            }
            catch (JavaScriptException e)
            {
                Script.IsValid = false;
                Script.Errors.Add(e.Message);
                ObjectFactory.GetProvider<ILoggingProvider>().Error(e.Message, e);
                throw;
            }
            catch (Exception e)
            {
                Script.IsValid = false;
                Script.Errors.Add(e.Message);
                ObjectFactory.GetProvider<ILoggingProvider>().Error(e.Message, e);
                throw;
            }
            IsRunning = false;
        }

        public void Dispose()
        {
            IsValid = false;
            HasBeenParsed = false;
            IsRunning = false;
            HasBeenExecuted = false;
            engine = null;
        }

        private void ExecutionCallback(bool b)
        {
            ObjectFactory.GetProvider<ILoggingProvider>().Debug("Execution callback called with bool "+ b);
            if (b)
                Run();
        }

        private void Expose()
        {
            engine.SetValue("scriptParameters", Script.Inputs.ToArray());
            engine.SetValue("executionCallback", new Action<bool>(ExecutionCallback));
            engine.SetValue("getKeyName", new Func<int, string>(GetKeyName));
            engine.SetValue("getKeyValue", new Func<string, int>(GetKeyValue));
            engine.SetValue("log", new Action<string>(DebugLog));
            engine.SetValue("pause", new Action<int>(Pause));

            engine.SetValue("getWindowRectangle", new Func<int[]>(GetWindowRectangle));
            engine.SetValue("getPixelColor", new Func<int, int, List<byte>>(GetPixelColor));
            engine.SetValue("moveMouse", new Action<int, int>(MoveMouse));
            engine.SetValue("moveMouseRelative", new Action<int, int>(MoveMouseRelative));

            engine.SetValue("clickMouse", new Action<int, int, Func<JsValue, JsValue[], JsValue>>(ClickMouse));
            engine.SetValue("clickMouseSync", new Action<int, int>(ClickMouseSync));

            engine.SetValue("sendInput", new Action<string, int, Func<JsValue, JsValue[], JsValue>>(SendInput));
            engine.SetValue("sendInputSync", new Action<string, int>(SendInputSync));
            engine.SetValue("sendInputMod", new Action<string, string[], int, Func<JsValue, JsValue[], JsValue>>(SendInputMod));
            engine.SetValue("sendInputModSync", new Action<string, string[], int>(SendInputModSync));


            engine.SetValue("sendString", new Action<string, int, Func<JsValue, JsValue[], JsValue>>(SendString));
            engine.SetValue("sendStringSync", new Action<string, int>(SendStringSync));
            engine.SetValue("setTargetProcess", new Action<string, bool>(SetTargetProcess));
        }

        private void SetSourceValid(SourceFile f)
        {
            f.CanExecute = true;
            f.Error = string.Empty;
            f.IsValid = true;
            f.ParseOk = true;
        }

        private void SetSourceInValid(SourceFile f, Exception e)
        {
            f.CanExecute = false;
            f.Error = e.Message;
            f.IsValid = false;
            f.ParseOk = false;
        }

        private List<byte> GetPixelColor(int x, int y)
        {
            var c = WindowsApi.GetPixelColor(x, y);
            return new List<byte>
            {
                c.R, c.G, c.B, c.A
            };
        }

        private int[] GetWindowRectangle()
        {
            var p = Sender.GetCurrentProcessTarget();
            return WindowsApi.GetWindowRectangle(p);
        }

        private void Pause(int ms)
        {
            Thread.Sleep(ms);
        }

        private void MoveMouse(int x, int y)
        {
            try
            {
                if (!testMode)
                    Sender.Send(new MouseSequenceItem(x, y, 1, -1, 0)).Wait(100);
            }
            catch (Exception e)
            {
                ObjectFactory.GetProvider<ILoggingProvider>().Error(e.Message, e);
            }
        }

        private void MoveMouseRelative(int x, int y)
        {
            try
            {
                if (!testMode)
                    Sender.Send(new MouseSequenceItem(x, y, 2, -1, 0)).Wait(100);
            }
            catch (Exception e)
            {
                ObjectFactory.GetProvider<ILoggingProvider>().Error(e.Message, e);
            }
        }

        private void ClickMouseSync(int b, int keepDown)
        {
            try
            {
                if (!testMode)
                    Sender.Send(new MouseSequenceItem(0, 0, 0, -b, keepDown)).Wait(keepDown);
            }
            catch (Exception e)
            {
                ObjectFactory.GetProvider<ILoggingProvider>().Error(e.Message, e);
            }
        }

        private async void ClickMouse(int b, int keepDown, Func<JsValue, JsValue[], JsValue> callBackFunction)
        {
            try
            {
                if (!testMode)
                    await Sender.Send(new MouseSequenceItem(0, 0, 0, -b, keepDown));
                callBackFunction(JsValue.Undefined, new[] {JsValue.Undefined});
            }
            catch (Exception e)
            {
                ObjectFactory.GetProvider<ILoggingProvider>().Error(e.Message, e);
            }
        }

        private void SendInputSync(string i, int keepDown)
        {
            try
            {
                if (!testMode)
                    Sender.Send(new SequenceItem(keepDown, new SequenceKeyValuePair(i, GetKeyValue(i)))).Wait(keepDown);
            }
            catch (Exception e)
            {
                ObjectFactory.GetProvider<ILoggingProvider>().Error(e.Message, e);
            }
        }

        private async void SendInput(string i, int keepDown, Func<JsValue, JsValue[], JsValue> callBackFunction)
        {
            try
            {
                if (!testMode)
                    await Sender.Send(new SequenceItem(keepDown, new SequenceKeyValuePair(i, GetKeyValue(i))));
                callBackFunction(JsValue.Undefined, new[] {JsValue.Undefined});
            }
            catch (Exception e)
            {
                ObjectFactory.GetProvider<ILoggingProvider>().Error(e.Message, e);
            }
        }

        private void SendInputModSync(string input, string[] modifiers, int keepDown)
        {
            try
            {
                if (!testMode)
                {
                    var sequence = new SequenceItem(keepDown, new SequenceKeyValuePair(input, GetKeyValue(input)));
                    foreach (var mod in modifiers)
                    {
                        sequence.Modifiers.Add(new SequenceKeyValuePair(mod, GetKeyValue(mod)));
                    }
                    Sender.Send(sequence).Wait(keepDown);
                }
            }
            catch (Exception e)
            {
                ObjectFactory.GetProvider<ILoggingProvider>().Error(e.Message, e);
            }
        }

        private async void SendInputMod(string input, string[] modifiers, int keepDown, Func<JsValue, JsValue[], JsValue> callBackFunction)
        {
            try
            {
                if (!testMode)
                {
                    var sequence = new SequenceItem(keepDown, new SequenceKeyValuePair(input, GetKeyValue(input)));
                    foreach (var mod in modifiers)
                    {
                        sequence.Modifiers.Add(new SequenceKeyValuePair(mod, GetKeyValue(mod)));
                    }
                    await Sender.Send(sequence);
                }
                callBackFunction(JsValue.Undefined, new[] { JsValue.Undefined });
            }
            catch (Exception e)
            {
                ObjectFactory.GetProvider<ILoggingProvider>().Error(e.Message, e);
            }
        }

        private List<SequenceItem> CreateSequenceList(string s, int ms)
        {
            var chars = s.ToCharArray();
            return chars.Select(c => new SequenceItem
            {
                KeepDown = ms, Entry = new SequenceKeyValuePair(c.ToString(), GetKeyValueFromCharacter(c))
            }).ToList();
        }

        private void SendStringSync(string s, int ms)
        {
            try
            {
                var sequences = CreateSequenceList(s, ms);
                if (!testMode)
                    Sender.Send(sequences.ToArray()).Wait(ms*sequences.Count);
            }
            catch (Exception e)
            {
                ObjectFactory.GetProvider<ILoggingProvider>().Error(e.Message, e);
            }
        }

        private async void SendString(string s, int ms, Func<JsValue, JsValue[], JsValue> callBackFunction)
        {
            try
            {
                var sequences = CreateSequenceList(s, ms);
                if (!testMode)
                    await Sender.Send(sequences.ToArray());
                callBackFunction(JsValue.Undefined, new[] {JsValue.Undefined});
            }
            catch (Exception e)
            {
                ObjectFactory.GetProvider<ILoggingProvider>().Error(e.Message, e);
            }
        }

        private void SetTargetProcess(string name, bool foreground)
        {
            Sender.SetTargetProcess(name, foreground);
        }

        private void DebugLog(string t)
        {
            ObjectFactory.GetProvider<ILoggingProvider>().Debug(t);
        }

        
        private string GetKeyName(int i)
        {
            return Enum.GetName(typeof(Keys), (Keys)i);
        }

        private int GetKeyValueFromCharacter(char c)
        {
            return (int)(Keys)(byte)char.ToUpper(c);
        }

        private int GetKeyValue(string name)
        {
            Keys e;
            if (!Keys.TryParse(name, out e))
            {
                return -1;
            }
            return (int)e;
        }
    }
}
