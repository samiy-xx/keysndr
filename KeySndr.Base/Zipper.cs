using System.IO;
using System.Linq;
using Ionic.Zip;
using Ionic.Zlib;
using KeySndr.Base.Providers;
using KeySndr.Common;

namespace KeySndr.Base
{
    public class Zipper : IZipper
    {
        private readonly IScriptProvider scriptProvider;
        private readonly IAppConfigProvider appConfigProvider;

        public Zipper(IScriptProvider s, IAppConfigProvider a)
        {
            scriptProvider = s;
            appConfigProvider = a;
        }

        public MemoryStream Zip(InputConfiguration inputConfig)
        {
            var stream = new MemoryStream();
            using (var zip = new ZipFile())
            {
                SetCompressionLevel(zip, 0);
                CreateFolders(zip, inputConfig.HasView);
                AddInputConfig(zip, inputConfig);

                foreach (var inputAction in inputConfig.Actions)
                {
                    if (!inputAction.HasScriptSequences)
                        continue;
                    AddScripts(zip, inputAction);
                }
                if (inputConfig.HasView)
                {
                    var path = Path.Combine(appConfigProvider.AppConfig.ViewsRoot, inputConfig.View);
                    if (new FileSystemUtils().DirectoryExists(path))
                        zip.AddDirectory(path, "Views\\" + inputConfig.View);
                }

                stream.Seek(0, SeekOrigin.Begin);
                zip.Save(stream);

            }
            stream.Seek(0, SeekOrigin.Begin);
            return stream;
        }

        private void SetCompressionLevel(ZipFile zip, int i)
        {
            zip.CompressionLevel = (CompressionLevel) i;
        }

        private void CreateFolders(ZipFile zip, bool hasView)
        {
            zip.AddDirectoryByName(KeySndrApp.ConfigurationsFolderName);
            zip.AddDirectoryByName(KeySndrApp.ScriptsFolderName);
            zip.AddDirectoryByName(KeySndrApp.MappingsFolderName);
            if (hasView)
                zip.AddDirectoryByName(KeySndrApp.ViewsFolderName);
        }

        private void AddInputConfig(ZipFile zip, InputConfiguration inputConfig)
        {
            var json = JsonSerializer.Serialize(inputConfig);
            zip.AddEntry(KeySndrApp.ConfigurationsFolderName + "\\" + inputConfig.FileName, json);
        }

        private void AddScripts(ZipFile zip, InputAction inputAction)
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
}
