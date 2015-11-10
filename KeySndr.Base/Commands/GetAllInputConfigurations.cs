using System;
using System.Collections.Generic;
using System.Linq;
using KeySndr.Base.Providers;
using KeySndr.Common;

namespace KeySndr.Base.Commands
{
    public class GetAllInputConfigurations : ICommand<ApiResult<IEnumerable<string>>>
    {
        private readonly IInputConfigProvider inputConfigProvider;
        public ApiResult<IEnumerable<string>> Result { get; private set; }

        public GetAllInputConfigurations(IInputConfigProvider p)
        {
            inputConfigProvider = p;
        }

        public void Execute()
        {
            try
            {
                Result = new ApiResult<IEnumerable<string>>
                {
                    Content = inputConfigProvider.Configs.Select(i => i.Name),
                    Success = true
                };
            }
            catch (Exception e)
            {
                Result = new ApiResult<IEnumerable<string>>
                {
                    Success = false,
                    Message = "Failed to get input configurations",
                    ErrorMessage = e.Message
                };
            }
        }
    }
}
