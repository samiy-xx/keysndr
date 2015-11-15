using System;
using System.Collections.Generic;
using KeySndr.Base.Domain;
using KeySndr.Base.Providers;
using KeySndr.Common;

namespace KeySndr.Base.Commands
{
    public class GetAllScriptObjects : ICommand<ApiResult<IEnumerable<InputScript>>>
    {
        private readonly IScriptProvider scriptProvider;
        public ApiResult<IEnumerable<InputScript>> Result { get; private set; }


        public GetAllScriptObjects(IScriptProvider s)
        {
            scriptProvider = s;
        }

        public void Execute()
        {
            try
            {
                Result = new ApiResult<IEnumerable<InputScript>>
                {
                    Content = scriptProvider.Scripts,
                    Success = true,
                    Message = "Ok"
                };
            }
            catch (Exception e)
            {
                Result = new ApiResult<IEnumerable<InputScript>>
                {
                    Content = new InputScript[0],
                    Success = false,
                    Message = "Error getting scripts",
                    ErrorMessage = e.Message
                };
            }
        }

    }
}
