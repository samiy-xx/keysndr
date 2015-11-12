using System;
using System.IO;
using KeySndr.Base.Providers;
using KeySndr.Common;

namespace KeySndr.Base.Commands
{
    public class RemoveInputConfiguration : ICommand<ApiResult<Object>>
    {
        private readonly IInputConfigProvider inputConfigProvider;
        private readonly IFileSystemProvider fileSystemProvider;
        private readonly IAppConfigProvider appConfigProvider;
        private readonly string configName;

        public ApiResult<Object> Result { get; private set; }

        public RemoveInputConfiguration(IInputConfigProvider p, IFileSystemProvider f, IAppConfigProvider a, string name)
        {
            inputConfigProvider = p;
            fileSystemProvider = f;
            appConfigProvider = a;
            configName = name;
        }

        public void Execute()
        {
            var config = inputConfigProvider.FindConfigForName(configName);
            
            if (config == null)
            {
                Result = new ApiResult<Object>
                {
                    Content = "Fail",
                    Success = false,
                    Message = "Fail",
                    ErrorMessage = $"Input configuration with name {configName} was not found"
                };
                return;
            }

            try
            {
                var pathToConfigs = appConfigProvider.AppConfig.ConfigFolder;
                var path = Path.Combine(pathToConfigs, config.FileName);
                fileSystemProvider.RemoveFile(path);
                inputConfigProvider.Remove(config);
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
                    Message = "Failed to remove configuration " + configName,
                    ErrorMessage = e.Message
                };
            }
        }
    }
}