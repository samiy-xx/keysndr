using System;
using System.Linq;
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
                SaveToStorage();
                inputConfigProvider.AddOrUpdate(configuration);
                
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

        private void SaveToStorage()
        {
            var existing = inputConfigProvider.Configs.FirstOrDefault(c => c.Equals(configuration));
            if (existing != null && !existing.FileName.Equals(configuration.FileName))
                storageProvider.UpdateInputConfiguration(configuration, existing);
            else
                storageProvider.SaveInputConfiguration(configuration);
        }
    }
}
