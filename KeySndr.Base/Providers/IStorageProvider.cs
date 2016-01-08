using System.Collections.Generic;
using KeySndr.Base.Domain;
using KeySndr.Common;
using KeySndr.Common.Providers;

namespace KeySndr.Base.Providers
{
    public interface IStorageProvider : IProvider
    {
        void Verify();
        void SaveInputConfiguration(InputConfiguration c);
        void UpdateInputConfiguration(InputConfiguration n, InputConfiguration o);
        void SaveScript(InputScript s);
        void UpdateScript(InputScript n, InputScript o);
        void RemoveInputConfiguration(InputConfiguration c);
        void RemoveScript(InputScript s);
        void LoadAllSourceFiles(InputScript s);
        void SaveScriptSource(InputScript script, string fileName, string source);
        IEnumerable<InputConfiguration> LoadInputConfigurations();
        IEnumerable<InputScript> LoadInputScripts();
    }
}
