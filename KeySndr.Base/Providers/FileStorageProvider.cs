using System;
using System.Collections.Generic;
using System.IO;
using KeySndr.Base.Domain;
using KeySndr.Common;

namespace KeySndr.Base.Providers
{
    public class FileStorageProvider : StorageProvider
    {
        public FileStorageProvider()
            : base()
        {
            
        }

        public override void Verify()
        {
            FileSystemUtils.Verify();
        }

        public override void SaveInputConfiguration(InputConfiguration c)
        {
            var path = AppConfigProvider.AppConfig.ConfigFolder;
            if (!FileSystemUtils.DirectoryExists(path))
                throw new Exception("Directory does not exist");
            FileSystemUtils.SaveObjectToDisk(c, Path.Combine(path, c.FileName));
            CreateViewFolder(c);
        }

        public override void UpdateInputConfiguration(InputConfiguration n, InputConfiguration o)
        {
            if (!HasFileNameChanged(n.FileName, o.FileName))
            {
                SaveInputConfiguration(n);
                return;
            }
            UpdateInputConfigurationFile(n, o);
        }

        private void UpdateInputConfigurationFile(InputConfiguration n, InputConfiguration o)
        {
            var path = AppConfigProvider.AppConfig.ConfigFolder;
            var oldConfigPath = Path.Combine(path, o.FileName);
            var newConfigPath = Path.Combine(path, n.FileName);
            if (!FileSystemUtils.FileExists(newConfigPath) && FileSystemUtils.FileExists(oldConfigPath))
                FileSystemUtils.MoveFile(oldConfigPath, newConfigPath);
            FileSystemUtils.SaveObjectToDisk(n, newConfigPath);
        }

        private void CreateViewFolder(InputConfiguration c)
        {
            if (!c.HasView)
                return;
            var path = Path.Combine(AppConfigProvider.AppConfig.ViewsRoot, c.View);
            if (!FileSystemUtils.DirectoryExists(path))
                FileSystemUtils.CreateDirectory(path);
        }

        public override void SaveScript(InputScript s)
        {
            var path = AppConfigProvider.AppConfig.ScriptsFolder;
            if (!FileSystemUtils.DirectoryExists(path))
                throw new Exception("Directory does not exist");
            FileSystemUtils.SaveObjectToDisk(s, Path.Combine(path, s.FileName));
            CreateSourceFilesDirectoryIfNotExists(s);
            CreatePlaceholderSourceFiles(s);
        }

        public override void UpdateScript(InputScript n, InputScript o)
        {
            if (!HasFileNameChanged(n.FileName, o.FileName))
            {
                SaveScript(n);
                return;
            }
            
            UpdateScriptDirectory(n, o);
            UpdateScriptFile(n, o);
        }

        private bool HasFileNameChanged(string n, string o)
        {
            return !n.Equals(o);
        }

        private void UpdateScriptDirectory(InputScript n, InputScript o)
        {
            var path = AppConfigProvider.AppConfig.ScriptsFolder;
            var oldFolderPath = Path.Combine(path, o.Name);
            var newFolderPath = Path.Combine(path, n.Name);
            if (!FileSystemUtils.DirectoryExists(newFolderPath) && FileSystemUtils.DirectoryExists(oldFolderPath))
                FileSystemUtils.MoveDirectory(oldFolderPath, newFolderPath);
        }

        private void UpdateScriptFile(InputScript n, InputScript o)
        {
            var path = AppConfigProvider.AppConfig.ScriptsFolder;
            var oldScriptPath = Path.Combine(path, o.FileName);
            var newScriptPath = Path.Combine(path, n.FileName);
            if (!FileSystemUtils.FileExists(newScriptPath) && FileSystemUtils.FileExists(oldScriptPath))
                FileSystemUtils.MoveFile(oldScriptPath, newScriptPath);
            FileSystemUtils.SaveObjectToDisk(n, newScriptPath);
        }

        private void CreatePlaceholderSourceFiles(InputScript s)
        {
            foreach (var sourceName in s.SourceFileNames)
            {
                var path = Path.Combine(AppConfigProvider.AppConfig.ScriptsFolder, s.Name, sourceName);
                if (!FileSystemUtils.FileExists(path))
                    FileSystemUtils.SaveStringToDisk("// Code away!!", path);
            }
        }

        public override void RemoveInputConfiguration(InputConfiguration i)
        {
            var path = AppConfigProvider.AppConfig.ConfigFolder;
            if (!FileSystemUtils.DirectoryExists(path))
                throw new Exception("Directory does not exist");
            FileSystemUtils.RemoveFile(Path.Combine(path, i.FileName));
        }

        public override void RemoveScript(InputScript s)
        {
            var path = AppConfigProvider.AppConfig.ScriptsFolder;
            if (!FileSystemUtils.DirectoryExists(path))
                throw new Exception("Directory does not exist");
            FileSystemUtils.RemoveFile(Path.Combine(path, s.FileName));
        }

        public override IEnumerable<InputConfiguration> LoadInputConfigurations()
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

        public override IEnumerable<InputScript> LoadInputScripts()
        {
            var c = new List<InputScript>();
            foreach (var file in GetAllScriptFiles())
            {
                var i = LoadInputScript(file);
                if (i == null)
                    continue;
                i.FileName = file;
                LoadAllSourceFiles(i);
                c.Add(i);
            }
            return c;
        }

        public override void Dispose()
        {
            
        }
        
        private IEnumerable<string> GetAllScriptFiles()
        {
            return string.IsNullOrEmpty(AppConfigProvider.AppConfig.ScriptsFolder) 
                ? new string[0] 
                : FileSystemUtils.GetDirectoryFileNames(AppConfigProvider.AppConfig.ScriptsFolder, "script", true);
        }

        private IEnumerable<string> GetAllConfigurationFiles()
        {
            return string.IsNullOrEmpty(AppConfigProvider.AppConfig.ConfigFolder) 
                ? new string[0] 
                : FileSystemUtils.GetDirectoryFileNames(AppConfigProvider.AppConfig.ConfigFolder, "json", true);
        }

        private InputScript LoadInputScript(string fileName)
        {
            return string.IsNullOrEmpty(AppConfigProvider.AppConfig.ScriptsFolder)
                ? null 
                : FileSystemUtils.LoadObjectFromDisk<InputScript>(Path.Combine(AppConfigProvider.AppConfig.ScriptsFolder, fileName));
        }

        private InputConfiguration LoadInputConfiguration(string fileName)
        {
            return string.IsNullOrEmpty(AppConfigProvider.AppConfig.ConfigFolder) 
                ? null 
                : FileSystemUtils.LoadObjectFromDisk<InputConfiguration>(Path.Combine(AppConfigProvider.AppConfig.ConfigFolder, fileName));
        }
    }
}
