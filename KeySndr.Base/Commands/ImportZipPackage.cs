using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KeySndr.Base.Providers;
using KeySndr.Common;

namespace KeySndr.Base.Commands
{
    public class ImportZipPackage : ICommand<ApiResult<Object>>
    {
        private readonly IInputConfigProvider inputConfigProvider;
        private readonly IScriptProvider scriptProvider;
        private readonly IAppConfigProvider appConfigProvider;

        public ApiResult<object> Result { get; private set; }

        public ImportZipPackage(IInputConfigProvider i, IScriptProvider s, IAppConfigProvider a)
        {
            inputConfigProvider = i;
            scriptProvider = s;
            appConfigProvider = a;
        }

        public void Execute()
        {
            throw new NotImplementedException();
        }
    }
}
