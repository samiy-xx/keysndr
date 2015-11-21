using System;
using System.Collections.Generic;
using System.Linq;
using KeySndr.Base.Domain;

namespace KeySndr.Base.Providers
{
    public class ScriptProvider : IScriptProvider
    {
        private readonly List<InputScript> scripts;
        private readonly List<IScriptContext> contexts;

        public IEnumerable<InputScript> Scripts => scripts;
        public IEnumerable<IScriptContext> Contexts => contexts;

        public ScriptProvider()
        {
            scripts = new List<InputScript>();
            contexts = new List<IScriptContext>();
        }

        public void AddScript(InputScript script, bool createContext = false)
        {
            lock (scripts)
            {
                if (scripts.Contains(script))
                    return;

                scripts.Add(script);
            }
            
            if (createContext)
                Create(script);
        }

        public void AddOrUpdate(InputScript script, bool createContext = false)
        {
            lock (scripts)
            {
                var index = scripts.IndexOf(script);
                if (index > 0)
                {
                    scripts[index] = script;
                }
                else
                {
                    scripts.Add(script);
                    if (createContext)
                        Create(script);
                }
            }
        }

        public void RemoveScript(InputScript script)
        {
            lock (scripts)
            {
                if (scripts.Contains(script))
                    scripts.Remove(script);
            }
        }

        public IScriptContext Create(InputScript script)
        {
            lock (contexts)
            {
                var ctx = new JintScriptingContext(script);
                contexts.Add(ctx);
                return ctx;
            }
        }

        public IScriptContext FindContextForName(string name)
        {
            lock (scripts)
            {
                var script = scripts.FirstOrDefault(c => c.Name == name);
                if (script == null)
                    throw new Exception("No such script found");

                return GetContext(script);
            }
        }

        public IScriptContext GetContext(InputScript script)
        {
            lock (contexts)
            {
                return contexts.FirstOrDefault(s => s.Script.Equals(script));
            }
        }

        public void Clear()
        {
            lock (scripts)
            {
                scripts.Clear();
            }
        }

        public void Dispose()
        {
            lock (scripts)
            {
                scripts.Clear();
            }

            lock (contexts)
            {
                contexts.Clear();
            }
        }
    }
}
