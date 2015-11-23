using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ionic.Zip;
using KeySndr.Base.Providers;
using KeySndr.Common;

namespace KeySndr.Base.Commands
{
    public class ImportZipPackage : ICommand<ApiResult<Object>>
    {
        private readonly IInputConfigProvider inputConfigProvider;
        private readonly IScriptProvider scriptProvider;
        private readonly IAppConfigProvider appConfigProvider;
        private readonly byte[] bytes;

        public ApiResult<object> Result { get; private set; }

        public ImportZipPackage(IInputConfigProvider i, IScriptProvider s, IAppConfigProvider a, byte[] b)
        {
            inputConfigProvider = i;
            scriptProvider = s;
            appConfigProvider = a;
            bytes = b;
        }

        public void Execute()
        {

            using (var zip = ZipFile.Read(new MemoryStream(bytes)))
            {
                if (zip.ContainsEntry(KeySndrApp.ConfigurationsFolderName + "/"))
                {
                    // Do something clever
                }
            }
        }
    }
}
