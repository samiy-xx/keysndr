using System.Collections.Generic;
using System.Threading.Tasks;

namespace KeySndr.Common.Providers
{
    public interface IWebConnectionProvider : IProvider
    {
        void SetBaseAddress(string ip, int port);
        Task<IEnumerable<string>> RequestConfigurations();
        Task<IEnumerable<string>> RequestScripts();
        Task<InputConfiguration> RequestConfiguration(string name);
        Task<bool> ExecuteAction(InputAction action);
        Task<bool> SaveConfiguration(InputConfiguration configuration);
    }
}
