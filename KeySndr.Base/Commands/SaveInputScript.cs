using System;
using System.Linq;
using KeySndr.Base.Domain;
using KeySndr.Base.Providers;
using KeySndr.Common;

namespace KeySndr.Base.Commands
{
    public class SaveInputScript : ICommand<ApiResult<Object>>
    {
        private readonly IAppConfigProvider appConfigProvider;
        private readonly IStorageProvider storageProvider;
        private readonly IScriptProvider scriptProvider;
        private readonly InputScript script;

        public ApiResult<Object> Result { get; private set; }

        public SaveInputScript(IStorageProvider fs, IAppConfigProvider a, IScriptProvider s, InputScript c)
        {
            storageProvider = fs;
            appConfigProvider = a;
            scriptProvider = s;
            script = c;
        }

        public void Execute()
        {
            try
            {
                if (!scriptProvider.Scripts.Contains(script))
                    scriptProvider.AddOrUpdate(script, true);
                storageProvider.SaveScript(script);
                Result = new ApiResult<object>
                {
                    Content = "empty",
                    Success = true,
                    Message = "Ok"
                };
            }
            catch (Exception e)
            {
                Result = new ApiResult<object>
                {
                    Content = "empty",
                    Success = false,
                    Message = "Failed to save script " + script.Name,
                    ErrorMessage = e.Message
                };
            }
        }


    }
}
