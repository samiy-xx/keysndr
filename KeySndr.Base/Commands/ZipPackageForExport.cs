using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using Ionic.Zip;
using Ionic.Zlib;
using KeySndr.Base.Providers;
using KeySndr.Common;

namespace KeySndr.Base.Commands
{
    public class ZipPackageForExport : ICommand<MemoryStream>
    {
        private readonly IFileSystemProvider fileSystemProvider;
        private readonly IInputConfigProvider inputConfigProvider;
        private readonly IAppConfigProvider appConfigProvider;
        private readonly IScriptProvider scriptProvider;
        private readonly string configName;
        public MemoryStream Result { get; set; }

        public ZipPackageForExport(IFileSystemProvider f, IInputConfigProvider i, IAppConfigProvider a, IScriptProvider s, string config)
        {
            fileSystemProvider = f;
            inputConfigProvider = i;
            appConfigProvider = a;
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
                zip.AddDirectoryByName("Config");
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


                            
                            zip.AddEntry("Scripts\\" + inputScript.FileName, JsonSerializer.Serialize(inputScript));
                        }
                    }
                }


                // zip.Save(Path.Combine(appConfigProvider.AppConfig.ConfigFolder, "test.zip"));
                stream.Seek(0, SeekOrigin.Begin);
                zip.Save(stream);
                
            }
            stream.Seek(0, SeekOrigin.Begin);

            //FileStream fileStream = File.Create(Path.Combine(appConfigProvider.AppConfig.ConfigFolder, "test.zip"), (int)stream.Length);
            // Initialize the bytes array with the stream length and then fill it with data
            //byte[] bytesInStream = new byte[stream.Length];
            //stream.Read(bytesInStream, 0, bytesInStream.Length);
            // Use write method to write to the file specified above
            //fileStream.Write(bytesInStream, 0, bytesInStream.Length);
            //fileStream.Close();
            Result = stream;

        }
    }
}
