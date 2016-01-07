using System;
using System.Threading.Tasks;
using KeySndr.Common;


namespace KeySndr.Base.Commands
{
    public class ExecuteInputAction : IAsyncCommand<ApiResult<Object>>
    {
        public ApiResult<Object> Result { get; private set; }
        private readonly InputActionExecutionContainer actionContainer;

        public ExecuteInputAction(InputActionExecutionContainer c)
        {
            actionContainer = c;
        }

        public async Task Execute()
        {
            await Task.Run(async () =>
            {
                try
                {
                    await new ActionProcessor(actionContainer).Process();
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
                        Message = "Failed to execute action " + actionContainer.InputAction.Name,
                        ErrorMessage = e.Message
                    };
                }
            });
            
        }
    }
}
