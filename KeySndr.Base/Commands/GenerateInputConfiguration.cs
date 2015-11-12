using KeySndr.Common;

namespace KeySndr.Base.Commands
{
    public class GenerateInputConfiguration : ICommand<ApiResult<InputConfiguration>>
    {
        private readonly int count;
        public ApiResult<InputConfiguration> Result { get; private set; }

        public GenerateInputConfiguration(int c)
        {
            count = c;
        }

        public void Execute()
        {
            Result = new ApiResult<InputConfiguration>
            {
                Content = new InputConfiguration("New Configuration", InputAction.GenerateFullSet(count)),
                Message = "OK",
                Success = true
            };
        }
    }
}
