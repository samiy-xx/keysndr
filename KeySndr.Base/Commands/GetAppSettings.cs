using KeySndr.Base.Domain;
using KeySndr.Base.Providers;
using KeySndr.Common;

namespace KeySndr.Base.Commands
{
    public class GetAppSettings : ICommand<ApiResult<AppConfig>>
    {
        private readonly IAppConfigProvider appConfigProvider;
        public ApiResult<AppConfig> Result { get; private set; }

        public GetAppSettings(IAppConfigProvider a)
        {
            appConfigProvider = a;
        }

        public void Execute()
        {
            Result = new ApiResult<AppConfig>
            {
                Content = appConfigProvider.AppConfig,
                Success = true,
                Message = "Ok"
            };
        }
    }
}
