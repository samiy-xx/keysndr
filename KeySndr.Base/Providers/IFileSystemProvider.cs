using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using KeySndr.Base.Domain;
using KeySndr.Common;

namespace KeySndr.Base.Providers
{
    public interface IFileSystemProvider : IProvider
    {
        void VerifyFolderStructure();

        T LoadObjectFromDisk<T>(string path);
        string LoadStringFromDisk(string path);
        IEnumerable<string> GetDirectoryFileNames(string path, bool fileNameWithoutPath = false);
        IEnumerable<FileInfo> GetDirectoryFiles(string path);
        IEnumerable<string> GetDirectoryFileNames(string path, string extension, bool fileNameWithoutPath = false);
        IEnumerable<string> GetAllConfigurationFiles(string path); 
        void RemoveFile(string path);
        void SaveObjectToDisk(object o, string path);
        AppConfig LoadAppConfiguration();
        void SaveAppConfiguration();
        InputConfiguration LoadInputConfiguration(string path);
    }
}
