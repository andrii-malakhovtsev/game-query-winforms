using System.IO;

namespace WhatGameToPlay
{
    public class PlayerFile : TextFile, IFileCreator
    {
        private readonly string _filePath;

        public PlayerFile(string name, PlayersDirectory directory) : base(name)
        {
            _filePath = directory.GetSelectedFilePath(_name);
            CreateFileIfMissing();
        }

        public void CreateFileIfMissing() => FilesCreator.CreateFileIfMissing(_filePath);

        public void Delete() => File.Delete(_filePath);
    }
}
