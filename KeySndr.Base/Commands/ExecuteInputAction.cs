using System;
using System.Threading.Tasks;
using KeySndr.Common;


namespace KeySndr.Base.Commands
{
    public class ExecuteInputAction : IAsyncCommand<ApiResult<Object>>
    {
        public ApiResult<Object> Result { get; private set; }
        private readonly InputActionExecutionContainer actionContainer;
        private readonly IActionProcessor processor;

        public ExecuteInputAction(InputActionExecutionContainer c)
        {
            actionContainer = c;
            processor = new ActionProcessor(actionContainer);
        }

        public ExecuteInputAction(IActionProcessor p, InputActionExecutionContainer c)
        {
            actionContainer = c;
            processor = p;
            p.Container = actionContainer;
        }

        public async Task Execute()
        {
            await Task.Run(async () =>
            {
                try
                {
                    await processor.Process();
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
