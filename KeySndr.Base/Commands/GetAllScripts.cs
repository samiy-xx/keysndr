using System;
using System.Collections.Generic;
using System.Linq;
using KeySndr.Base.Providers;

namespace KeySndr.Base.Commands
{
    public class GetAllScripts : ICommand<IEnumerable<string>>
    {
        private readonly IScriptProvider scriptProvider;

        public IEnumerable<string> Result { get; private set; }
        public bool Success { get; private set; }

        public GetAllScripts(IScriptProvider s)
        {
            scriptProvider = s;
        }

        public void Execute()
        {
            try
            {
                Result = scriptProvider.Scripts.Select(s => s.Name);
                Success = true;
            }
            catch (Exception e)
            {
                Success = false;
            }
        }

    }
}
