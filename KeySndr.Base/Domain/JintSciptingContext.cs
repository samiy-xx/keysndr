﻿using System;
using System.Collections.Generic;
using System.Linq;
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
            Expose();
        }

        public JintScriptingContext(InputScript script)
            : this()
        {
            Script = script;
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
            engine.SetValue("defaultKeyDownMs", 250);
            engine.SetValue("executionCallback", new Action<bool>(ExecutionCallback));
            engine.SetValue("getKeyName", new Func<int, string>(GetKeyName));
            engine.SetValue("getKeyValue", new Func<string, int>(GetKeyValue));
            engine.SetValue("log", new Action<string>(DebugLog));
            engine.SetValue("pause", new Action<int>(Pause));

            engine.SetValue("moveMouse", new Action<int, int>(MoveMouse));
            engine.SetValue("moveMouseRelative", new Action<int, int>(MoveMouseRelative));

            engine.SetValue("clickMouse", new Action<int, int, Func<Jint.Native.JsValue, Jint.Native.JsValue[], Jint.Native.JsValue>>(ClickMouse));
            engine.SetValue("clickMouseSync", new Action<int, int>(ClickMouseSync));

            engine.SetValue("sendInput", new Action<string, int, Func<Jint.Native.JsValue, Jint.Native.JsValue[], Jint.Native.JsValue>>(SendInput));
            engine.SetValue("sendInputSync", new Action<string, int>(SendInputSync));
           
            engine.SetValue("sendString", new Action<string, int, Func<Jint.Native.JsValue, Jint.Native.JsValue[], Jint.Native.JsValue>>(SendString));
            engine.SetValue("sendStringSync", new Action<string, int>(SendStringSync));
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

        private void Pause(int ms)
        {
            Thread.Sleep(ms);
        }

        private void MoveMouse(int x, int y)
        {
            var a = new InputAction
            {
                Name = $"Move Mouse to {x} {y}"
            };
            a.MouseSequences.Add(new MouseSequenceItem(x, y, 1, -1, 0));
            if (!testMode)
                Sender.Send(a).Wait(100);
        }

        private void MoveMouseRelative(int x, int y)
        {
            var a = new InputAction
            {
                Name = $"Move Mouse Relative {x} {y}"
            };
            a.MouseSequences.Add(new MouseSequenceItem(x, y, 2, -1, 0));
            if (!testMode)
                Sender.Send(a).Wait(100);
        }

        private void ClickMouseSync(int b, int keepDown)
        {
            if (!testMode)
                Sender.Send(new MouseSequenceItem(0, 0, 0, -b, keepDown)).Wait(keepDown);
        }

        private async void ClickMouse(int b, int keepDown, Func<Jint.Native.JsValue, Jint.Native.JsValue[],
                       Jint.Native.JsValue> callBackFunction)
        {
            if (!testMode)
                await Sender.Send(new MouseSequenceItem(0, 0, 0, -b, keepDown));
            callBackFunction(JsValue.Undefined, new[] { JsValue.Undefined });
        }

        private void SendInputSync(string i, int keepDown)
        {
            Sender.Send(new SequenceItem(keepDown, new SequenceKeyValuePair(i, GetKeyValue(i)))).Wait(keepDown);
        }

        private async void SendInput(string i, int keepDown, Func<Jint.Native.JsValue, Jint.Native.JsValue[],
                       Jint.Native.JsValue> callBackFunction)
        {
            await Sender.Send(new SequenceItem(keepDown, new SequenceKeyValuePair(i, GetKeyValue(i))));
            callBackFunction(JsValue.Undefined, new[] { JsValue.Undefined });
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
            var sequences = CreateSequenceList(s, ms);
            Sender.Send(sequences.ToArray()).Wait(ms * sequences.Count);
        }

        private async void SendString(string s, int ms, Func<Jint.Native.JsValue, Jint.Native.JsValue[],
                       Jint.Native.JsValue> callBackFunction)
        {
            var sequences = CreateSequenceList(s, ms);
            await Sender.Send(sequences.ToArray());
            callBackFunction(JsValue.Undefined, new[] { JsValue.Undefined });
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