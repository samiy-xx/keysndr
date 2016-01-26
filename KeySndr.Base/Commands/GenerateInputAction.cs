using KeySndr.Common;

namespace KeySndr.Base.Commands
{
    public class GenerateInputAction : ICommand<ApiResult<InputAction>>
    {
        public ApiResult<InputAction> Result { get; private set; }

        public void Execute()
        {
            Result = new ApiResult<InputAction>
            {
                Content = new InputAction(),
                Message = "OK",
                Success = true
            };
        }
    }
}
