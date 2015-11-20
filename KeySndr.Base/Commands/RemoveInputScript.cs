using System;
using KeySndr.Base.Domain;
using KeySndr.Base.Providers;
using KeySndr.Common;

namespace KeySndr.Base.Commands
{
    public class RemoveInputScript : ICommand<ApiResult<Object>>
    {
        private readonly IScriptProvider scriptProvider;
        private readonly IStorageProvider storageProvider;
        private readonly InputScript script;

        public ApiResult<Object> Result { get; private set; }

        public RemoveInputScript(IScriptProvider p, IStorageProvider f, InputScript s)
        {
            scriptProvider = p;
            storageProvider = f;
            script = s;
        }

        public void Execute()
        {
            try
            {
                scriptProvider.RemoveScript(script);
                storageProvider.RemoveScript(script);
                Result = new ApiResult<Object>
                {
                    Content = "OK",
                    Success = true,
                    Message = "Ok"
                };

            }
            catch (Exception e)
            {
                Result = new ApiResult<Object>
                {
                    Success = false,
                    Message = "Failed to remove script " + script.Name,
                    ErrorMessage = e.Message
                };
            }
        }
    }
}