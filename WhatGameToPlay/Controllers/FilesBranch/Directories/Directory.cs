using System.IO;

namespace WhatGameToPlay
{
    public abstract class Directory : StorageItem
    {
        protected Directory(string name) : base(name) {}

        public string Name => _name;
        
        protected string GetFullDirectoryFilePath(string fileName)
            => GetFullDirectoryTextFilePath(_name, fileName + TextFileExtension);

        public string GetSelectedFilePath(string fileName) => GetFullDirectoryTextFilePath(_name, fileName);

        private static string GetFullDirectoryTextFilePath(string directory, string fileName)
            => directory + "\\" + fileName;

        public abstract void CreateDirectoryIfNotExists();

        public bool FileExists(string fileName) => FilesReader.TextFileExist(TextFiles, fileName);

        protected FileInfo[] TextFiles => GetTextFiles(directoryName: _name);

        private static FileInfo[] GetTextFiles(string directoryName)
        {
            const string textFilesInDirectory = "*" + TextFileExtension;
            var directory = new DirectoryInfo(directoryName);
            return directory.GetFiles(textFilesInDirectory);
        }
    }
}
