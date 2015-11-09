using System;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;
using Jint;
using Jint.Runtime;
using KeySndr.Base.Exceptions;
using KeySndr.Common;

namespace KeySndr.Base.Domain
{
    public class JintScriptingContext : IScriptContext
    {
        //private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly Engine engine;

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

            engine = new Engine(cfg =>
                cfg.AllowClr(typeof(InputAction).Assembly)
                .AllowClr(typeof(SequenceItem).Assembly));

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
                    //Log.Info("Executing script " + sourceFile.FileName);
                    engine.Execute(sourceFile.Contents);

                    HasBeenExecuted = true;
                    IsValid = true;
                    HasBeenParsed = true;

                    SetSourceValid(sourceFile);
                }
                catch (Exception e)
                {
                    //Log.Error("Executing script failed", e);
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
            if (!HasBeenExecuted)
                throw new ScriptException("Script has not been executed");

            //Log.Info("Invoking script " + Script.Name);
            IsRunning = true;
            while (true)
            {
                object o;
                try
                {
                    o = engine.Invoke("execute").ToObject();
                    engine.Invoke("executionCallback");
                    Script.IsValid = true;
                    Script.Errors.Clear();
                }
                catch (JavaScriptException e)
                {
                    //Log.Error("Invoking script failed", e);
                    Script.IsValid = false;
                    Script.Errors.Add(e.Message);
                    throw;
                }
                catch (Exception e)
                {
                    //Log.Error("Invoking script failed", e);
                    Script.IsValid = false;
                    Script.Errors.Add(e.Message);
                    throw;
                }

                Thread.Sleep(50);

                if (!(o is bool))
                    return;

                var ready = (bool)o;

                if (!ready)
                    continue;
                break;
            }
            IsRunning = false;
        }

        private void Expose()
        {
            engine.SetValue("defaultKeyDownMs", 250);

            engine.SetValue("getKeyName", new Func<int, string>(GetKeyName));
            engine.SetValue("getKeyValue", new Func<string, int>(GetKeyValue));
            engine.SetValue("log", new Action<string>(DebugLog));

            engine.SetValue("sendInput", new Action<InputAction>(SendInputAction));
            engine.SetValue("sendString", new Action<string, int>(SendString));

            engine.SetValue("createAction", new Func<InputAction>(CreateAction));
            engine.SetValue("createStringAction", new Func<string, int, InputAction>(CreateStringAction));
            engine.SetValue("createSequenceFromString", new Func<int, string, SequenceItem>(CreateSequence));
            engine.SetValue("createSequenceFromInt", new Func<int, int, SequenceItem>(CreateSequence));
            engine.SetValue("appendSequence", new Action<InputAction, SequenceItem>(AppendSequence));
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

        public async void SendInputAction(InputAction action)
        {
            Debug.WriteLine("Action " + action.Name + " executed");
            if (!testMode)
                await Sender.Send(action);
        }

        public InputAction CreateAction()
        {
            return new InputAction();
        }

        public SequenceItem CreateSequence(int keepDown, string keyName)
        {
            return new SequenceItem(keepDown, GetKeyValue(keyName), keyName);
        }

        public SequenceItem CreateSequence(int keepDown, int keyValue)
        {
            return new SequenceItem(keepDown, keyValue, GetKeyName(keyValue));
        }

        public void AppendSequence(InputAction action, SequenceItem item)
        {
            action.Sequences.Add(item);
        }

        public InputAction CreateStringAction(string s, int ms)
        {
            var a = new InputAction
            {
                Name = "String action"
            };
            var chars = s.ToCharArray();
            foreach (var c in chars)
            {
                a.Sequences.Add(new SequenceItem
                {
                    KeepDown = ms,
                    Entry = new SequenceKeyValuePair(c.ToString(), GetKeyValueFromCharacter(c))
                });
            }
            return a;
        }

        public void SendString(string s, int ms)
        {
            SendInputAction(CreateStringAction(s, ms));
        }

        public void DebugLog(string t)
        {
            Debug.WriteLine(t);
        }

        public string GetKeyName(int i)
        {
            return Enum.GetName(typeof(Keys), (Keys)i);
        }

        public int GetKeyValueFromCharacter(char c)
        {
            return (int)(Keys)(byte)char.ToUpper(c);
        }

        public int GetKeyValue(string name)
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
