using System.Collections.Generic;
using System.Threading.Tasks;
using MacroToolServer.CommonPortable;

namespace KeySndr.Base.Providers
{
    public interface IInputConfigProvider : IProvider
    {
        void Add(InputConfiguration config);
        //Task Store(InputConfiguration config);
        IEnumerable<InputConfiguration> Configs { get; }
        InputConfiguration FindConfigForName(string name);
        void Clear();
    }
}
