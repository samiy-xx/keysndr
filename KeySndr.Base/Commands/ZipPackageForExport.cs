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
        private readonly string configName;
        public MemoryStream Result { get; set; }

        public ZipPackageForExport(IInputConfigProvider i, IScriptProvider s, string config)
        {
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
                zip.AddDirectoryByName("Configurations");
                zip.AddDirectoryByName("Scripts");
                zip.AddDirectoryByName("Maps");
                zip.AddEntry("Config\\" + inputConfig.FileName, JsonSerializer.Serialize(inputConfig));
                
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

                            zip.AddDirectoryByName("Scripts\\" + inputScript.Name);

                            foreach (var sourceFile in inputScript.SourceFiles)
                            {
                                zip.AddEntry("Scripts\\" + inputScript.Name + "\\" + inputScript.FileName, sourceFile.Contents);
                            }

                            zip.AddEntry("Scripts\\" + inputScript.FileName, JsonSerializer.Serialize(inputScript));
                        }
                    }
                }

                stream.Seek(0, SeekOrigin.Begin);
                zip.Save(stream);
                
            }
            stream.Seek(0, SeekOrigin.Begin);
            Result = stream;

        }
    }
}
