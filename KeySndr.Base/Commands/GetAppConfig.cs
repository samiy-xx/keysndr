using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KeySndr.Base.Domain;
using KeySndr.Base.Providers;

namespace KeySndr.Base.Commands
{
    public class GetAppConfig : ICommand<AppConfig>
    {
        public AppConfig Result { get; private set; }
        public bool Success { get; private set; }
        public void Execute()
        {
            var acp = ObjectFactory.GetProvider<IAppConfigProvider>();
            Success = true;
            Result = acp.AppConfig;
        }
    }
}
