using System.Collections.Generic;
using System.IO;
using KeySndr.Base.Domain;
using KeySndr.Common;

namespace KeySndr.Base
{
    public interface IFileSystemUtils
    {
        AppConfig AppConfig { get; }
        string AppDataFolder { get; }

        void CreateDirectory(string path);
        bool DirectoryExists(string path);
        void Dispose();
        bool FileExists(string path);
        IEnumerable<string> GetAllConfigurationFiles(string path);
        IEnumerable<string> GetDirectoryFileNames(string path, bool fileNameWithoutPath = false);
        IEnumerable<string> GetDirectoryFileNames(string path, string extension, bool fileNameWithoutPath = false);
        IEnumerable<string> GetDirectoryFileNames(string path, string[] extensions, bool fileNameWithoutPath = false);
        IEnumerable<FileInfo> GetDirectoryFiles(string path);
        AppConfig LoadAppConfiguration();
        InputConfiguration LoadInputConfiguration(string path);
        T LoadObjectFromDisk<T>(string path);
        string LoadStringFromDisk(string path);
        void MoveDirectory(string path, string newPath);
        void MoveFile(string path, string newPath);
        void RemoveFile(string path);
        void SaveAppConfiguration();
        void SaveObjectToDisk(object o, string path);
        void SaveStringToDisk(string text, string path);
        void Verify();
    }
}