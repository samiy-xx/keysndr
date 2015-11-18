using System;
using KeySndr.Common;

namespace KeySndr.Base.Commands
{
    public class ReloadSystem : ICommand<ApiResult<Object>>
    {
        public ApiResult<object> Result { get; private set; }
        public void Execute()
        {
            Result = new ApiResult<Object>
            {
                Success = true,
                Content = "Ok",
                Message = "Restarted"
            };
            TempEventNotifier.ReloadRequested();
        }
    }
}
