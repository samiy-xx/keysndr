using System;
using KeySndr.Base.Domain;
using KeySndr.Base.Providers;
using KeySndr.Common;

namespace KeySndr.Base.Commands
{
    public class StoreAppConfig : ICommand<ApiResult<Object>>
    {
        private readonly AppConfig config;
        private readonly FileSystemUtils fileUtils;
        private readonly IAppConfigProvider appConfigProvider;

        public ApiResult<Object> Result { get; private set; }

        public StoreAppConfig(IAppConfigProvider a, AppConfig c)
        {
            fileUtils = new FileSystemUtils();
            appConfigProvider = a;
            config = c;
        }

        public void Execute()
        {
            try
            {
                if (config.FirstTimeRunning)
                    config.FirstTimeRunning = false;

                appConfigProvider.AppConfig = config;
                fileUtils.SaveAppConfiguration();

                Result = new ApiResult<object>
                {
                    Content = "OK",
                    Success = true,
                    Message = "Ok"
                };
            }
            catch (Exception e)
            {
                Result = new ApiResult<object>
                {
                    Content = "Fail",
                    Success = false,
                    Message = "Failed to save appconfig",
                    ErrorMessage = e.Message
                };
            }
        }
    }
}
