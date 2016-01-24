using System.Linq;
using KeySndr.Base.Dto;
using KeySndr.Base.Providers;
using KeySndr.Common;

namespace KeySndr.Base.Commands
{
    public class LoadSource : ICommand<ApiResult<string>>
    {
        public ApiResult<string> Result { get; private set; }
        private readonly IScriptProvider scriptProvider;
        private readonly GetSourceRequest request;

        public LoadSource(IScriptProvider s, GetSourceRequest script)
        {
            scriptProvider = s;
            request = script;
        }

        public void Execute()
        {

            var script = scriptProvider.Scripts.FirstOrDefault(s => s.Equals(request.Script));
            var source = script?.SourceFiles.FirstOrDefault(s => s.FileName.Equals(request.SourceFileName));
            if (source != null)
            {
                Result = new ApiResult<string>
                {
                    Content = source.Contents,
                    Success = true,
                    Message = "Ok"
                };
                return;
            }

            Result = new ApiResult<string>
            {
                Content = "empty",
                Success = false,
                Message = "Failed to load source " + request.SourceFileName,
                ErrorMessage = "No source found"
            };
        }
    }
}
