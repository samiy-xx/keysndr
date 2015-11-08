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
            if (scripts.Contains(script))
                return;

            scripts.Add(script);
            if (createContext)
                Create(script);
        }

        public void RemoveScript(InputScript script)
        {
            if (scripts.Contains(script))
                scripts.Remove(script);
        }

        public IScriptContext Create(InputScript script)
        {
            var ctx = new JintScriptingContext(script);
            contexts.Add(ctx);
            return ctx;
        }

        public IScriptContext FindContextForName(string name)
        {
            var script = scripts.FirstOrDefault(c => c.Name == name);
            if (script == null)
                throw new Exception("No such script found");

            return GetContext(script);
            //if (script.Context != null)
            //    return script.Context;

            var ctx = Create(script);

            //script.Context = ctx;
            return ctx;
        }

        public IScriptContext GetContext(InputScript script)
        {
            var found = contexts.FirstOrDefault(s => s.Script == script);
            //if (found == null)
            //return FindContextForName(script.FileName);
            return found;
        }

        public void Clear()
        {
            scripts.Clear();
        }
    }
}
