using KeySndr.Base.Domain;
using KeySndr.Common;

namespace KeySndr.Base.Commands
{
    public class GetAssemblyVersion : ICommand<ApiResult<string>>
    {
        public ApiResult<string> Result { get; private set; }

        public void Execute()
        {
            Result = new ApiResult<string>
            {
                Content = AssemblyInfo.GetAssemblyVersion(typeof(KeySndrApp)).ToString(),
                Message = "OK",
                Success = true
            };
        }
    }
}
