using System;
using System.Collections.Generic;
using System.IO;
using KeySndr.Base.Domain;
using KeySndr.Common;
using KeySndr.Common.Extensions;

namespace KeySndr.Base.Providers
{
    public class FileStorageProvider : IStorageProvider
    {
        protected readonly IFileSystemUtils FileSystemUtils;
        protected readonly IAppConfigProvider AppConfigProvider;

        public FileStorageProvider()
        {
            FileSystemUtils = new FileSystemUtils();
            AppConfigProvider = ObjectFactory.GetProvider<IAppConfigProvider>();
        }

        public FileStorageProvider(IFileSystemUtils fs, IAppConfigProvider a)
        {
            FileSystemUtils = fs;
            AppConfigProvider = a;
        }

        public void Verify()
        {
            FileSystemUtils.Verify();
        }

        public virtual void SaveInputConfiguration(InputConfiguration c)
        {
            var path = AppConfigProvider.AppConfig.ConfigFolder;
            if (!FileSystemUtils.DirectoryExists(path))
                throw new Exception("Directory does not exist");
            FileSystemUtils.SaveObjectToDisk(c, Path.Combine(path, c.FileName));
        }

        public void CreateViewFolder(string viewFolderName)
        {
            var path = Path.Combine(AppConfigProvider.AppConfig.ViewsRoot, viewFolderName);
            if (!FileSystemUtils.DirectoryExists(path))
                FileSystemUtils.CreateDirectory(path);
        }

        public void CreateMediaFolder(string mediaFolderName)
        {
            var path = Path.Combine(AppConfigProvider.AppConfig.MediaRoot, mediaFolderName);
            if (!FileSystemUtils.DirectoryExists(path))
                FileSystemUtils.CreateDirectory(path);
        }

        public IEnumerable<string> LoadMediaFileNames(InputConfiguration c)
        {
            var path = Path.Combine(AppConfigProvider.AppConfig.MediaRoot, c.Name.RemoveWhitespace());
            if (!FileSystemUtils.DirectoryExists(path))
                return new List<string>();
            
            var fileNames = FileSystemUtils.GetDirectoryFileNames(path, new []{"png", "jpg", "jpeg", "gif"}, true);
            return fileNames;
        }
        
        public void UpdateInputConfiguration(InputConfiguration n, InputConfiguration o)
        {
            var path = AppConfigProvider.AppConfig.ConfigFolder;
            var oldConfigPath = Path.Combine(path, o.FileName);
            var newConfigPath = Path.Combine(path, n.FileName);
            if (!FileSystemUtils.FileExists(newConfigPath) && FileSystemUtils.FileExists(oldConfigPath))
                FileSystemUtils.MoveFile(oldConfigPath, newConfigPath);
            FileSystemUtils.SaveObjectToDisk(n, newConfigPath);
        }

        public void SaveScript(InputScript s)
        {
            var path = AppConfigProvider.AppConfig.ScriptsFolder;
            if (!FileSystemUtils.DirectoryExists(path))
                throw new Exception("Directory does not exist");
            FileSystemUtils.SaveObjectToDisk(s, Path.Combine(path, s.FileName));
            CreateSourceFilesDirectoryIfNotExists(s);
            CreatePlaceholderSourceFiles(s);
        }

        protected void CreateSourceFilesDirectoryIfNotExists(InputScript s)
        {
            if (!HasSourceFilesDirectory(s))
                CreateSourceFilesDirectory(s);
        }

        protected void CreateSourceFilesDirectory(InputScript s)
        {
            var path = Path.Combine(AppConfigProvider.AppConfig.ScriptsFolder, s.Name);
            FileSystemUtils.CreateDirectory(path);
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

        public void UpdateScript(InputScript n, InputScript o)
        {
            UpdateScriptDirectory(n, o);
            UpdateScriptFile(n, o);
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

        public void SaveScriptSource(InputScript script, string fileName, string source)
        {
            var scriptsPath = AppConfigProvider.AppConfig.ScriptsFolder;
            var sourcePath = Path.Combine(scriptsPath, script.Name, fileName);
            FileSystemUtils.SaveStringToDisk(source, sourcePath);
        }

        private bool HasFileNameChanged(string n, string o)
        {
            return !n.Equals(o);
        }

        

        

        

        public void RemoveInputConfiguration(InputConfiguration i)
        {
            var path = AppConfigProvider.AppConfig.ConfigFolder;
            if (!FileSystemUtils.DirectoryExists(path))
                throw new Exception("Directory does not exist");
            FileSystemUtils.RemoveFile(Path.Combine(path, i.FileName));
        }

        public void RemoveScript(InputScript s)
        {
            var path = AppConfigProvider.AppConfig.ScriptsFolder;
            if (!FileSystemUtils.DirectoryExists(path))
                throw new Exception("Directory does not exist");
            FileSystemUtils.RemoveFile(Path.Combine(path, s.FileName));
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
                LoadAllSourceFiles(i);
                c.Add(i);
            }
            return c;
        }

        public void Dispose()
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
            InputScript script = null;
            try
            {
                script = string.IsNullOrEmpty(AppConfigProvider.AppConfig.ScriptsFolder)
                    ? null
                    : FileSystemUtils.LoadObjectFromDisk<InputScript>(
                        Path.Combine(AppConfigProvider.AppConfig.ScriptsFolder, fileName));
            }
            catch (Exception e)
            {
                ObjectFactory.GetProvider<ILoggingProvider>().Error(e.Message, e);
            }
            return script;
        }

        private InputConfiguration LoadInputConfiguration(string fileName)
        {
            return string.IsNullOrEmpty(AppConfigProvider.AppConfig.ConfigFolder) 
                ? null 
                : FileSystemUtils.LoadObjectFromDisk<InputConfiguration>(Path.Combine(AppConfigProvider.AppConfig.ConfigFolder, fileName));
        }

        public void LoadAllSourceFiles(InputScript s)
        {
            s.SourceFiles.Clear();
            foreach (var sourceFile in s.SourceFileNames)
            {
                if (!SourceFileExists(s, sourceFile))
                    continue;
                var p = GetPathToSourceFile(s, sourceFile);
                if (p == null)
                    continue;

                s.SourceFiles.Add(LoadSourceFile(p));
            }
        }

        protected SourceFile LoadSourceFile(string path)
        {
            return new SourceFile
            {
                Contents = FileSystemUtils.LoadStringFromDisk(path),
                FileName = Path.GetFileName(path)
            };
        }

        

        protected bool SourceFileExists(InputScript s, string f)
        {
            var pathToFile = GetPathToSourceFile(s, f);
            return pathToFile != null && FileSystemUtils.FileExists(pathToFile);
        }

        protected bool HasSourceFilesDirectory(InputScript s)
        {
            var path = Path.Combine(AppConfigProvider.AppConfig.ScriptsFolder, s.Name);
            return FileSystemUtils.DirectoryExists(path);
        }

        

        protected string GetPathToSourceFile(InputScript s, string f)
        {
            if (!HasSourceFilesDirectory(s))
                return null;

            var pathToSourceFileDirectory = Path.Combine(AppConfigProvider.AppConfig.ScriptsFolder, s.Name);
            if (!FileSystemUtils.DirectoryExists(pathToSourceFileDirectory))
                return null;

            var fileName = Path.GetFileName(f);
            return fileName == null ? null : Path.Combine(pathToSourceFileDirectory, fileName);
        }
    }
}
