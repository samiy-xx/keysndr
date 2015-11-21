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
        void SaveScript(InputScript s);
        void RemoveInputConfiguration(InputConfiguration c);
        void RemoveScript(InputScript s);
        void LoadAllSourceFiles(InputScript s);
        IEnumerable<InputConfiguration> LoadInputConfigurations();
        IEnumerable<InputScript> LoadInputScripts();
    }
}
