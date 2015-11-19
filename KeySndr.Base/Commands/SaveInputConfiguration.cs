using System;
using KeySndr.Base.Providers;
using KeySndr.Common;

namespace KeySndr.Base.Commands
{
    public class SaveInputConfiguration : ICommand<ApiResult<Object>>
    {
        private readonly IStorageProvider storageProvider;
        private readonly IAppConfigProvider appConfigProvider;
        private readonly IInputConfigProvider inputConfigProvider;

        private readonly InputConfiguration configuration;

        public ApiResult<Object> Result { get; private set; }

        public SaveInputConfiguration(IStorageProvider fs, IAppConfigProvider a, IInputConfigProvider i, InputConfiguration c)
        {
            storageProvider = fs;
            appConfigProvider = a;
            inputConfigProvider = i;
            configuration = c;
        }

        public void Execute()
        {
            try
            {

                inputConfigProvider.AddOrUpdate(configuration);
                storageProvider.SaveInputConfiguration(configuration);
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
                    Message = "Failed to save input configuration "+ configuration.Name,
                    ErrorMessage = e.Message
                };
            }
        }


    }
}
