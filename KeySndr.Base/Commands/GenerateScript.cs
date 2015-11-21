using KeySndr.Base.Domain;
using KeySndr.Common;

namespace KeySndr.Base.Commands
{
    public class GenerateScript : ICommand<ApiResult<InputScript>>
    {
        public ApiResult<InputScript> Result { get; private set; }

        public GenerateScript()
        {
        }

        public void Execute()
        {
            Result = new ApiResult<InputScript>
            {
                Content = new InputScript()
                {
                    Name = "New Script"
                },
                Message = "OK",
                Success = true
            };
        }
    }
}
