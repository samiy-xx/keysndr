using System.IO;
using System.Linq;
using Ionic.Zip;
using Ionic.Zlib;
using KeySndr.Base.Providers;
using KeySndr.Common;

namespace KeySndr.Base.Commands
{
    public class ZipPackageForExport : ICommand<MemoryStream>
    {

        private readonly IInputConfigProvider inputConfigProvider;
        private readonly IScriptProvider scriptProvider;
        private readonly IAppConfigProvider appConfigProvider;

        private readonly string configName;
        public MemoryStream Result { get; set; }

        public ZipPackageForExport(IInputConfigProvider i, IScriptProvider s, IAppConfigProvider a, string config)
        {
            appConfigProvider = a;
            inputConfigProvider = i;
            scriptProvider = s;
            configName = config;
        }

        public void Execute()
        {
            var inputConfig = inputConfigProvider.FindConfigForName(configName);
            if (inputConfig == null)
                return;


            var stream = new MemoryStream();
            using (var zip = new ZipFile())
            {
                zip.CompressionLevel = CompressionLevel.Level0;
                zip.AddDirectoryByName(KeySndrApp.ConfigurationsFolderName);
                zip.AddDirectoryByName(KeySndrApp.ScriptsFolderName);
                zip.AddDirectoryByName(KeySndrApp.MappingsFolderName);
                

                zip.AddEntry(KeySndrApp.ConfigurationsFolderName + "\\" + inputConfig.FileName, JsonSerializer.Serialize(inputConfig));
                
                foreach (var inputAction in inputConfig.Actions)
                {
                    if (inputAction.HasScriptSequences)
                    {
                        foreach (var scriptSequenceItem in inputAction.ScriptSequences)
                        {
                            var inputScript =
                                scriptProvider.Scripts.FirstOrDefault(s => s.Name == scriptSequenceItem.ScriptName);
                            if (inputScript == null)
                                continue;

                            zip.AddDirectoryByName(KeySndrApp.ScriptsFolderName + "\\Sources");

                            foreach (var sourceFile in inputScript.SourceFiles)
                            {
                                zip.AddEntry(KeySndrApp.ScriptsFolderName + "\\Sources\\" + sourceFile.FileName, sourceFile.Contents);
                            }

                            zip.AddEntry(KeySndrApp.ScriptsFolderName + "\\" + inputScript.FileName, JsonSerializer.Serialize(inputScript));
                        }
                    }
                    
                }
                if (inputConfig.HasView)
                {
                    zip.AddDirectoryByName("View");
                    var path = Path.Combine(appConfigProvider.AppConfig.WebRoot, inputConfig.View);
                    if (new FileSystemUtils().DirectoryExists(path))
                        zip.AddDirectory(path, "View");
                }

                stream.Seek(0, SeekOrigin.Begin);
                zip.Save(stream);
                
            }
            stream.Seek(0, SeekOrigin.Begin);
            Result = stream;

        }
    }
}
