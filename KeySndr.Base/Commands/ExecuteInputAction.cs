using System;
using KeySndr.Common;


namespace KeySndr.Base.Commands
{
    public class ExecuteInputAction : ICommand<ApiResult<Object>>
    {
        public ApiResult<Object> Result { get; private set; }
        private readonly InputAction action;

        public ExecuteInputAction(InputAction a)
        {
            action = a;
        }

        public void Execute()
        {
            try
            {
                Sender.Send(action).Wait(1000);
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
                    Message = "Failed to execute action "+ action.Name,
                    ErrorMessage = e.Message
                };
            }

        }


    }
}
