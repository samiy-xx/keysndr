using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Ionic.Zip;
using KeySndr.Base.Domain;
using KeySndr.Base.Providers;
using KeySndr.Common;

namespace KeySndr.Base.Commands
{
    public class ImportZipPackage : IAsyncCommand<ApiResult<Object>>
    {
        private readonly IAppConfigProvider appConfigProvider;
        private readonly IInputConfigProvider inputConfigProvider;
        private readonly IScriptProvider scriptProvider;
        private readonly IStorageProvider storageProvider;

        private readonly byte[] bytes;

        public ApiResult<object> Result { get; private set; }

        public ImportZipPackage(IInputConfigProvider i, IScriptProvider s, IStorageProvider t, IAppConfigProvider a, byte[] b)
        {
            inputConfigProvider = i;
            scriptProvider = s;
            appConfigProvider = a;
            storageProvider = t;
            bytes = b;
        }

        public async Task Execute()
        {
            using (var zip = ZipFile.Read(new MemoryStream(bytes)))
            {
                var configurations = GetConfigurations(zip);
                var scripts = GetScripts(zip);
                var maps = GetMaps(zip);

                SaveConfigurations(configurations, zip);
                SaveScripts(scripts);
                SaveMaps(maps);

                await inputConfigProvider.Prepare();
                await scriptProvider.Prepare();
            }
        }

        private IEnumerable<InputConfiguration> GetConfigurations(ZipFile zip)
        {
            var listOut = new List<InputConfiguration>();
            var entries = zip.SelectEntries("*.json", KeySndrApp.ConfigurationsFolderName + "/");
            foreach (var zipEntry in entries)
            {
                using (var stream = new MemoryStream())
                {
                    zipEntry.Extract(stream);
                    stream.Seek(0, SeekOrigin.Begin);
                    var reader = new StreamReader(stream);
                    var c = JsonSerializer.Deserialize<InputConfiguration>(reader.ReadToEnd());
                    listOut.Add(c);
                }
            }
            return listOut;
        }

        private IEnumerable<InputScript> GetScripts(ZipFile zip)
        {
            var listOut = new List<InputScript>();
            var entries = zip.SelectEntries("*", KeySndrApp.ScriptsFolderName + "/");
            foreach (var zipEntry in entries)
            {
                var stream = new MemoryStream();
                zipEntry.Extract(stream);
                stream.Seek(0, SeekOrigin.Begin);
                var reader = new StreamReader(stream);
                InputScript script;
                if (zipEntry.FileName.EndsWith(".script"))
                {
                    script = JsonSerializer.Deserialize<InputScript>(reader.ReadToEnd());
                    listOut.Add(script);
                }
            }
            return listOut;
        }

        private IEnumerable<InputMap> GetMaps(ZipFile zip)
        {
            var listOut = new List<InputMap>();

            return listOut;
        }

        private void SaveConfigurations(IEnumerable<InputConfiguration> configurations, ZipFile zip)
        {
            foreach (var configuration in configurations)
            {
                inputConfigProvider.AddOrUpdate(configuration);
                storageProvider.SaveInputConfiguration(configuration);

                if (configuration.HasView)
                {
                    SaveView(zip, appConfigProvider.AppConfig.WebRoot);
                }
            }    
        }

        private void SaveScripts(IEnumerable<InputScript> scripts)
        {
            foreach (var script in scripts)
            {
                scriptProvider.AddOrUpdate(script, true);
                storageProvider.SaveScript(script);
            }
        }

        private void SaveMaps(IEnumerable<InputMap> maps)
        {
            foreach (var map in maps)
            {
                
            }
        }

        private void SaveView(ZipFile zip, string path)
        {
            var entries = zip.Entries.Where(e => e.FileName.StartsWith(KeySndrApp.ViewsFolderName + "/"));
            foreach (var entry in entries)
            {
                entry.Extract(path, ExtractExistingFileAction.OverwriteSilently);
            }
        }

        //
    }
}
