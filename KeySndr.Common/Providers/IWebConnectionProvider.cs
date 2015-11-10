using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KeySndr.Common.Providers
{
    public interface IWebConnectionProvider : IProvider
    {
        void SetBaseAddress(string ip, int port);
        Task<ApiResult<IEnumerable<string>>> RequestConfigurations();
        Task<ApiResult<IEnumerable<string>>> RequestScripts();
        Task<ApiResult<InputConfiguration>> RequestConfiguration(string name);
        Task<ApiResult<Object>> ExecuteAction(InputAction action);
        Task<ApiResult<Object>> SaveConfiguration(InputConfiguration configuration);
    }
}
