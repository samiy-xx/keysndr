using System.IO;
using KeySndr.Base.Providers;

namespace KeySndr.Base.Commands
{
    public class ZipPackageForExport : ICommand<MemoryStream>
    {
        private readonly IInputConfigProvider inputConfigProvider;
        private readonly IScriptProvider scriptProvider;
        private readonly IAppConfigProvider appConfigProvider;
        private readonly string configName;
        private readonly IZipper zipper;

        public MemoryStream Result { get; set; }

        public ZipPackageForExport(IInputConfigProvider i, IScriptProvider s, IAppConfigProvider a, string config)
        {
            appConfigProvider = a;
            inputConfigProvider = i;
            scriptProvider = s;
            configName = config;
            zipper = new Zipper(scriptProvider, appConfigProvider);
        }

        public ZipPackageForExport(IInputConfigProvider i, IScriptProvider s, IAppConfigProvider a, IZipper z, string config)
        {
            appConfigProvider = a;
            inputConfigProvider = i;
            scriptProvider = s;
            configName = config;
            zipper = z;
        }

        public void Execute()
        {
            var inputConfig = inputConfigProvider.FindConfigForName(configName);
            if (inputConfig == null)
                return;
            
            Result = zipper.Zip(inputConfig);
        }
    }
}
