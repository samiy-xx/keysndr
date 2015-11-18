using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using KeySndr.Base.Domain;
using KeySndr.Base.Exceptions;
using KeySndr.Common;
using Newtonsoft.Json;

namespace KeySndr.Base.Providers
{
    public class FileSystemProvider : IFileSystemProvider
    {
        private const string AppConfigName = "app.json";
        private const string ConfigurationFileExtension = ".json";
        private const string AppDataFolderName = "KeySndr";

        public string AppDataFolder => Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                AppDataFolderName);

        public AppConfig AppConfig => ObjectFactory.GetProvider<IAppConfigProvider>().AppConfig;

        public FileSystemProvider()
        {
            
        }

        public void Verify()
        {
            VerifyFolderStructure();
            VerifyDataFolderStructure();
        }

        private void VerifyFolderStructure()
        {
            if (!Directory.Exists(AppDataFolder))
                Directory.CreateDirectory(AppDataFolder);
        }

        private void VerifyDataFolderStructure()
        {
            if (string.IsNullOrEmpty(AppConfig.DataFolder) || !Directory.Exists(AppConfig.DataFolder))
                throw new DataFolderDoesNotExistException();

            if (!Directory.Exists(AppConfig.DataFolder))
                Directory.CreateDirectory(AppConfig.DataFolder);

            if (!Directory.Exists(AppConfig.ConfigFolder))
                Directory.CreateDirectory(AppConfig.ConfigFolder);

            if (!Directory.Exists(AppConfig.ScriptsFolder))
                Directory.CreateDirectory(AppConfig.ScriptsFolder);

            if (!Directory.Exists(AppConfig.WebRoot))
                Directory.CreateDirectory(AppConfig.WebRoot);
        }

        public T LoadObjectFromDisk<T>(string path)
        {
            if (!File.Exists(path))
                return default(T);

            using (var reader = new StreamReader(path))
            {
                return JsonConvert.DeserializeObject<T>(reader.ReadToEnd());
            }
        }

        public string LoadStringFromDisk(string path)
        {
            if (!File.Exists(path))
                return null;
            using (var reader = new StreamReader(path))
            {
                return reader.ReadToEnd();
            }
        }

        public void SaveObjectToDisk(object o, string path)
        {
            File.WriteAllText(path, JsonConvert.SerializeObject(o, Formatting.Indented), Encoding.UTF8);
        }

        public IEnumerable<string> GetDirectoryFileNames(string path, bool fileNameWithoutPath = false)
        {
            if (!Directory.Exists(path)) 
                return new string[0];

            var files = Directory.GetFiles(path);
            if (fileNameWithoutPath)
                return files.Select(Path.GetFileName);
            return files;
        }

        public IEnumerable<string> GetDirectoryFileNames(string path, string extension, bool fileNameWithoutPath = false)
        {
            return GetDirectoryFileNames(path, fileNameWithoutPath)
                .Where(f => f.EndsWith(extension));
        }

        public IEnumerable<string> GetAllConfigurationFiles(string path)
        {
            return GetDirectoryFileNames(path, "json").Select(Path.GetFileNameWithoutExtension);
        } 

        public IEnumerable<FileInfo> GetDirectoryFiles(string path)
        {
            return !Directory.Exists(path) 
                ? new FileInfo[0] 
                : Directory.GetFiles(path).Select(f => new FileInfo(f));
        }

        public void RemoveFile(string path)
        {
            if (!File.Exists(path))
                return;
            
            File.Delete(path);
        }

        public void SaveAppConfiguration()
        {
            File.WriteAllText(
                Path.Combine(AppDataFolder, AppConfigName),
                JsonConvert.SerializeObject(ObjectFactory.GetProvider<IAppConfigProvider>().AppConfig, Formatting.Indented),
                Encoding.UTF8);
        }

        public AppConfig LoadAppConfiguration()
        {
            var path = Path.Combine(AppDataFolder, AppConfigName);
            if (!File.Exists(path))
                return null;

            using (var reader = new StreamReader(Path.Combine(AppDataFolder, AppConfigName)))
            {
                return JsonConvert.DeserializeObject<AppConfig>(reader.ReadToEnd());
            }
        }

        public InputConfiguration LoadInputConfiguration(string path)
        {
            if (!File.Exists(path))
                return null;

            return LoadObjectFromDisk<InputConfiguration>(path);
        }

        public void Dispose()
        {
            
        }
    }
}
