using System.IO;

namespace WhatGameToPlay
{
    public abstract class Directory : StorageItem
    {
        protected Directory(string name) : base(name) {}

        public string Name => _name;

        protected static string GetFullDirectoryFilePath(string directory, string fileName)  // mb change to non-static later
                => GetFullDirectoryTextFilePath(directory, fileName + TextFileExtension);

        public string GetSelectedFilePath(string fileName) => GetFullDirectoryTextFilePath(_name, fileName);

        protected static string GetFullDirectoryTextFilePath(string directory, string fileName)
                => directory + "\\" + fileName;

        public abstract void CreateDirectoryIfNotExists();

        public abstract FileInfo[] TextFiles { get; }

        protected static FileInfo[] GetTextFiles(string directoryName)
        {
            const string textFilesInDirectory = "*" + TextFileExtension;
            var directory = new DirectoryInfo(directoryName);
            return directory.GetFiles(textFilesInDirectory);
        }
    }
}
