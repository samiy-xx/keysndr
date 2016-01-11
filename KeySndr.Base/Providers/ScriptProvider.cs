using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
                if (index > -1)
                {
                    var ctx = GetContext(scripts[index]);
                    if (ctx != null)
                    {
                        RemoveContext(ctx);
                        ctx.Dispose();
                    }
                    
                    scripts[index] = script;
                    Create(script);
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

        private void RemoveContext(IScriptContext ctx)
        {
            lock (contexts)
            {
                contexts.Remove(ctx);
            }    
        }

        public void Clear()
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

        public async Task Prepare()
        {
            var storageProvider = ObjectFactory.GetProvider<IStorageProvider>();
            Clear();

            await Task.Run(async () =>
            {
                foreach (var s in storageProvider.LoadInputScripts())
                {
                    AddScript(s, true);
                    await s.RunTest();
                }
            });
        }

        public void Dispose()
        {
            foreach (var scriptContext in contexts)
            {
                scriptContext.Dispose();
            }
            Clear();
        }
    }
}
