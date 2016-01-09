using KeySndr.Base.Providers;
using KeySndr.Common;

namespace KeySndr.Base.Commands
{
    public class GetInputConfiguration : ICommand<ApiResult<InputConfiguration>>
    {
        private readonly IInputConfigProvider inputConfigProvider;
        private readonly IAppConfigProvider appConfigProvider;
        private readonly string configName;

        public ApiResult<InputConfiguration> Result { get; private set; }

        public GetInputConfiguration(IInputConfigProvider p, string name)
        {
            inputConfigProvider = p;
            configName = name;
        }

        public void Execute()
        {
            var c = inputConfigProvider.FindConfigForName(configName);
            if (c != null)
            {
                Result = new ApiResult<InputConfiguration>
                {
                    Content = c,
                    Success = true,
                    Message = "Ok"
                };
                return;
            }

            Result = new ApiResult<InputConfiguration>
            {
                    Success = false,
                    Message = "Failed to get input configuration " + configName,
                    ErrorMessage = "Failed to get input configuration " + configName
            };
        }
    }
}
