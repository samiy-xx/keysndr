using System;
using KeySndr.Base.Domain;
using KeySndr.Base.Providers;
using KeySndr.Common;

namespace KeySndr.Base.Commands
{
    public class ValidateInputScript : ICommand<ApiResult<Object>>
    {
        private readonly IScriptProvider scriptProvider;
        private readonly InputScript script;

        public ApiResult<Object> Result { get; private set; }

        public ValidateInputScript(IScriptProvider s, InputScript c)
        {
            
            scriptProvider = s;
            script = c;
        }

        public void Execute()
        {
            Result = new ApiResult<object>
            {
                Content = "empty",
                Success = false,
                Message = "Script validation not finished.. Fix this",
                ErrorMessage = "Fail"
            };
            
        }


    }
}
