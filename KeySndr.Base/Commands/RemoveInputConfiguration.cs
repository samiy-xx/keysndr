using System;
using KeySndr.Base.Providers;
using KeySndr.Common;

namespace KeySndr.Base.Commands
{
    public class RemoveInputConfiguration : ICommand<ApiResult<Object>>
    {
        private readonly IInputConfigProvider inputConfigProvider;
        private readonly IStorageProvider storageProvider;
        private readonly IAppConfigProvider appConfigProvider;
        private readonly string configName;

        public ApiResult<Object> Result { get; private set; }

        public RemoveInputConfiguration(IInputConfigProvider p, IStorageProvider f, IAppConfigProvider a, string name)
        {
            inputConfigProvider = p;
            storageProvider = f;
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
                if (config.HasView)
                {
                    // TODO: Remove view folder and files
                }
                storageProvider.RemoveInputConfiguration(config);
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