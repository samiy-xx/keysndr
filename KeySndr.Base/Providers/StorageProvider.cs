using System.Collections.Generic;
using System.IO;
using KeySndr.Base.Domain;
using KeySndr.Common;

namespace KeySndr.Base.Providers
{
    public abstract class StorageProvider : IStorageProvider
    {
        protected readonly FileSystemUtils FileSystemUtils;
        protected readonly IAppConfigProvider AppConfigProvider;

        protected StorageProvider()
        {
            FileSystemUtils = new FileSystemUtils();
            AppConfigProvider = ObjectFactory.GetProvider<IAppConfigProvider>();
        }

        public abstract void Dispose();
        public abstract void Verify();
        public abstract void SaveInputConfiguration(InputConfiguration c);
        public abstract void UpdateInputConfiguration(InputConfiguration n, InputConfiguration o);
        public abstract void SaveScript(InputScript s);
        public abstract void UpdateScript(InputScript n, InputScript o);
        public abstract void RemoveInputConfiguration(InputConfiguration c);
        public abstract void RemoveScript(InputScript s);
        public abstract IEnumerable<InputConfiguration> LoadInputConfigurations();
        public abstract IEnumerable<InputScript> LoadInputScripts();

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

        protected void CreateSourceFilesDirectoryIfNotExists(InputScript s)
        {
            if (!HasSourceFilesDirectory(s))
                CreateSourceFilesDirectory(s);
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

        protected void CreateSourceFilesDirectory(InputScript s)
        {
            var path = Path.Combine(AppConfigProvider.AppConfig.ScriptsFolder, s.Name);
            FileSystemUtils.CreateDirectory(path);
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
