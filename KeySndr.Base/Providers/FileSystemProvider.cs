using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using KeySndr.Base.Domain;
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

        public void VerifyFolderStructure()
        {
            if (!Directory.Exists(AppDataFolder))
                Directory.CreateDirectory(AppDataFolder);
        }

        public IEnumerable<string> GetAllConfigurationFiles()
        {
            IEnumerable<string> files;
            try
            {

                files = Directory.GetFiles(AppConfig.ConfigFolder, "*.json")
                    .Where(s => s.EndsWith(ConfigurationFileExtension)).Select(Path.GetFileNameWithoutExtension);
            }
            catch (IOException)
            {
                files = new List<string>();
            }
            return files;
        }

        public IEnumerable<string> GetAllScriptFiles()
        {
            IEnumerable<string> files;
            try
            {

                files = Directory.GetFiles(AppConfig.ScriptsFolder, "*.script")
                    .Select(Path.GetFileName);
            }
            catch (IOException)
            {
                files = new List<string>();
            }
            return files;
        }

        public void RemoveFile(string path)
        {
            try
            {
                File.Delete(path);
            }
            catch (Exception e)
            {

            }
        }
        public AppConfig LoadAppConfiguration()
        {


            try
            {
                var reader =
                    new StreamReader(Path.Combine(AppDataFolder, AppConfigName));
                var res = reader.ReadToEnd();
                reader.Dispose();
                return JsonConvert.DeserializeObject<AppConfig>(res);
            }
            catch (Exception)
            {
                return null;
            }

        }

        public void SaveAppConfiguration(AppConfig c)
        {
            File.WriteAllText(
                Path.Combine(AppDataFolder, AppConfigName),
                JsonConvert.SerializeObject(c, Formatting.Indented),
                Encoding.UTF8);
        }

        public async Task SaveInputConfiguration(InputConfiguration c)
        {
            var name = c.Name.Trim().Replace(" ", string.Empty);
            var path = Path.Combine(AppConfig.ConfigFolder, name + ConfigurationFileExtension);
            await SaveObjectToDiskAsJson(c, path);
        }

        /*public async Task SaveScript(InputScript s)
        {
            var path = Path.Combine(AppConfig.ConfigFolder, s.FileName);
            await SaveObjectToDiskAsJson(s, path);
        }*/

        public InputConfiguration LoadInputConfigurationFromDisk(string n)
        {
            var name = n.Trim().Replace(" ", string.Empty);
            var path = Path.Combine(AppConfig.ConfigFolder, name + ConfigurationFileExtension);
            return LoadObjectFromDiskAsJson<InputConfiguration>(path);
        }

        public async Task SaveObjectToDiskAsJson<T>(T o, string path)
        {
            var output = JsonConvert.SerializeObject(o, Formatting.Indented);
            await Task.Run(() =>
            {
                try
                {
                    File.WriteAllText(path, output, Encoding.UTF8);
                }
                catch (Exception e)
                {

                }
            });
        }

        public void SaveObjectToDisk<T>(T o, string path)
        {
            var writer = new XmlSerializer(typeof(T));
            using (var file = File.Create(path))
            {
                writer.Serialize(file, o);
            }
        }

        public T LoadObjectFromDisk<T>(string path)
        {
            var serializer = new XmlSerializer(typeof(T));
            using (var reader = new StreamReader(path))
            {
                return (T)serializer.Deserialize(reader);
            }

        }

        public string LoadFileAsString(string path)
        {
            using (var reader = new StreamReader(path))
            {
                return reader.ReadToEnd();
            }
        }

        public string LoadObjectFromDiskAsString(string path)
        {
            using (var reader = new StreamReader(Path.Combine(AppConfig.ScriptsFolder, path)))
            {
                return reader.ReadToEnd();
            }
        }

        public T LoadObjectFromDiskAsJson<T>(string path)
        {
            using (var reader = new StreamReader(path))
            {
                return JsonConvert.DeserializeObject<T>(reader.ReadToEnd());
            }
        }
    }
}
