using System.Collections.Generic;
using System.Threading.Tasks;
using KeySndr.Common;
using KeySndr.Common.Providers;

namespace KeySndr.Base.Providers
{
    public interface IInputConfigProvider : IProvider
    {
        void Add(InputConfiguration config);
        void AddOrUpdate(InputConfiguration config);
        void Remove(InputConfiguration config);
        IEnumerable<InputConfiguration> Configs { get; }
        InputConfiguration FindConfigForName(string name);
        void Clear();

        Task Prepare();
    }
}
