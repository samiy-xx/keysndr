using System;
using System.IO;
using System.Linq;
using KeySndr.Base.Domain;
using KeySndr.Base.Providers;
using KeySndr.Common;

namespace KeySndr.Base.Commands
{
    public class SaveInputScript : ICommand<ApiResult<Object>>
    {
        private readonly IAppConfigProvider appConfigProvider;
        private readonly IFileSystemProvider fileSystemProvider;
        private readonly IScriptProvider scriptProvider;
        private readonly InputScript script;

        public ApiResult<Object> Result { get; private set; }

        public SaveInputScript(IFileSystemProvider fs, IAppConfigProvider a, IScriptProvider s, InputScript c)
        {
            fileSystemProvider = fs;
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
                fileSystemProvider.SaveObjectToDisk(script, Path.Combine(appConfigProvider.AppConfig.ScriptsFolder, script.FileName));
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
