using System;
using System.Collections.Generic;
using KeySndr.Base.Providers;
using KeySndr.Common;

namespace KeySndr.Base.Commands
{
    public class GetProcessNames : ICommand<ApiResult<IEnumerable<ProcessInformation>>>
    {
        private readonly ISystemProvider systemProvider;
        public ApiResult<IEnumerable<ProcessInformation>> Result { get; private set; }

        public GetProcessNames(ISystemProvider p)
        {
            systemProvider = p;
        }

        public void Execute()
        {
            try
            {
                Result = new ApiResult<IEnumerable<ProcessInformation>>
                {
                    Content = systemProvider.ProcessNames(),
                    Success = true,
                    Message = "Ok"
                };
            }
            catch (Exception e)
            {
                Result = new ApiResult<IEnumerable<ProcessInformation>>
                {
                    Content = new ProcessInformation[0],
                    Success = false,
                    Message = "Failed to get process names",
                    ErrorMessage = e.Message
                };
            }
        }
    }
}
