using System.Collections.Generic;
using System.Threading.Tasks;
using KeySndr.Base.Domain;
using KeySndr.Common;

namespace KeySndr.Base.Providers
{
    public interface IFileSystemProvider : IProvider
    {
        void VerifyFolderStructure();
        IEnumerable<string> GetAllConfigurationFiles();
        IEnumerable<string> GetAllScriptFiles();
        AppConfig LoadAppConfiguration();
        void SaveAppConfiguration(AppConfig c);
        Task SaveInputConfiguration(InputConfiguration c);
        //Task SaveScript(InputScript s);
        InputConfiguration LoadInputConfigurationFromDisk(string n);
        Task SaveObjectToDiskAsJson<T>(T o, string path);
        void SaveObjectToDisk<T>(T o, string path);
        T LoadObjectFromDisk<T>(string path);
        string LoadObjectFromDiskAsString(string path);
        string LoadFileAsString(string path);
        T LoadObjectFromDiskAsJson<T>(string path);
        void RemoveFile(string path);
    }
}
