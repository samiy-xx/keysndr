using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KeySndr.Base.Domain;
using KeySndr.Common;

namespace KeySndr.Base.Providers
{
    public class FileStorageProvider : IStorageProvider
    {
        private readonly IFileSystemProvider fileSystemProvider;
        private readonly IAppConfigProvider appConfigProvider;

        public FileStorageProvider()
        {
            fileSystemProvider = ObjectFactory.GetProvider<IFileSystemProvider>();
            appConfigProvider = ObjectFactory.GetProvider<IAppConfigProvider>();
        }

        public void Verify()
        {
            fileSystemProvider.Verify();
        }

        public void SaveInputConfiguration(InputConfiguration c)
        {
            var path = appConfigProvider.AppConfig.ConfigFolder;
            if (!fileSystemProvider.DirectoryExists(path))
                throw new Exception("Directory does not exist");
            fileSystemProvider.SaveObjectToDisk(c, Path.Combine(path, c.FileName));
        }

        public void SaveScript(InputScript s)
        {
            var path = appConfigProvider.AppConfig.ScriptsFolder;
            if (!fileSystemProvider.DirectoryExists(path))
                throw new Exception("Directory does not exist");
            fileSystemProvider.SaveObjectToDisk(s, Path.Combine(path, s.FileName));
        }

        public void RemoveInputConfiguration(InputConfiguration i)
        {
            var path = appConfigProvider.AppConfig.ConfigFolder;
            if (!fileSystemProvider.DirectoryExists(path))
                throw new Exception("Directory does not exist");
            fileSystemProvider.RemoveFile(Path.Combine(path, i.FileName));
        }

        public void RemoveScript(InputScript s)
        {
            var path = appConfigProvider.AppConfig.ScriptsFolder;
            if (!fileSystemProvider.DirectoryExists(path))
                throw new Exception("Directory does not exist");
            fileSystemProvider.RemoveFile(Path.Combine(path, s.FileName));
        }

        public IEnumerable<InputConfiguration> LoadInputConfigurations()
        {
            var c = new List<InputConfiguration>();
            foreach (var file in GetAllConfigurationFiles())
            {
                var i = LoadInputConfiguration(file);
                if (i == null)
                    continue;
                i.FileName = file;
                c.Add(i);
            }
            return c;
        }

        public IEnumerable<InputScript> LoadInputScripts()
        {
            var c = new List<InputScript>();
            foreach (var file in GetAllScriptFiles())
            {
                var i = LoadInputScript(file);
                if (i == null)
                    continue;
                i.FileName = file;
                c.Add(i);
            }
            return c;
        }

        public void Dispose()
        {
            
        }

        private IEnumerable<string> GetAllScriptFiles()
        {
            return string.IsNullOrEmpty(appConfigProvider.AppConfig.ScriptsFolder) 
                ? new string[0] 
                : fileSystemProvider.GetDirectoryFileNames(appConfigProvider.AppConfig.ScriptsFolder, "script", true);
        }

        private IEnumerable<string> GetAllConfigurationFiles()
        {
            return string.IsNullOrEmpty(appConfigProvider.AppConfig.ConfigFolder) 
                ? new string[0] 
                : fileSystemProvider.GetDirectoryFileNames(appConfigProvider.AppConfig.ConfigFolder, "json", true);
        }

        private InputScript LoadInputScript(string fileName)
        {
            return string.IsNullOrEmpty(appConfigProvider.AppConfig.ScriptsFolder)
                ? null 
                : fileSystemProvider.LoadObjectFromDisk<InputScript>(Path.Combine(appConfigProvider.AppConfig.ScriptsFolder, fileName));
        }

        private InputConfiguration LoadInputConfiguration(string fileName)
        {
            return string.IsNullOrEmpty(appConfigProvider.AppConfig.ConfigFolder) 
                ? null 
                : fileSystemProvider.LoadObjectFromDisk<InputConfiguration>(Path.Combine(appConfigProvider.AppConfig.ConfigFolder, fileName));
        }
    }
}
