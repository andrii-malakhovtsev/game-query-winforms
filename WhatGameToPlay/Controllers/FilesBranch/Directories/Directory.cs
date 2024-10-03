using System.IO;

namespace WhatGameToPlay
{
    public abstract class Directory : StorageItem
    {
        protected Directory(string name) : base(name) {}

        public string Name => _name;

        protected string GetFilePath(string fileName)
        {
            return GetRelativeTextFilePath(_name, fileName + TextFileExtension);
        }

        public string GetSelectedFilePath(string fileName) => GetRelativeTextFilePath(_name, fileName);

        private static string GetRelativeTextFilePath(string directory, string fileName)
        {
            return directory + "\\" + fileName;
        }

        public abstract void CreateDirectoryIfMissing();

        public bool FileExists(string fileName) => FilesReader.TextFileExist(TextFiles, fileName);

        protected FileInfo[] TextFiles => GetTextFiles(directoryName: _name);

        private static FileInfo[] GetTextFiles(string directoryName)
        {
            const string TextFilesInDirectory = "*" + TextFileExtension;
            var directory = new DirectoryInfo(directoryName);
            return directory.GetFiles(TextFilesInDirectory);
        }
    }
}
