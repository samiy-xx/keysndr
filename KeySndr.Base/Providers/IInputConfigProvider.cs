using System.Collections.Generic;
using KeySndr.Common;

namespace KeySndr.Base.Providers
{
    public interface IInputConfigProvider : IProvider
    {
        void Add(InputConfiguration config);
        void Remove(InputConfiguration config);
        //Task Store(InputConfiguration config);
        IEnumerable<InputConfiguration> Configs { get; }
        InputConfiguration FindConfigForName(string name);
        
        void Clear();
    }
}
