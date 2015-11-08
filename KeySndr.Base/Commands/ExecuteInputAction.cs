using System;
using MacroToolServer.CommonPortable;

namespace KeySndr.Base.Commands
{
    public class ExecuteInputAction : ICommand<String>
    {
        public string Result { get; private set; }
        public bool Success { get; private set; }

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
                Result = "Sent";
                Success = true;
            }
            catch (Exception e)
            {
                Success = false;
                Result = e.Message;
            }

        }


    }
}
