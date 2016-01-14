using System.Collections.Generic;
using System.Threading.Tasks;
using KeySndr.Base.Domain;
using KeySndr.Common.Providers;

namespace KeySndr.Base.Providers
{
    public interface IScriptProvider : IProvider
    {
        void AddScript(InputScript script, bool createContext);
        void AddOrUpdate(InputScript script, bool createContext);
        void RemoveScript(InputScript script);
        IScriptContext Create(InputScript script);
        IEnumerable<InputScript> Scripts { get; }
        IEnumerable<IScriptContext> Contexts { get; }
        IScriptContext GetContext(InputScript script);
        IScriptContext FindContextForName(string scriptName);
        void Clear();

        Task Prepare();
    }
}
