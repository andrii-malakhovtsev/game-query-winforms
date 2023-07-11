using System.IO;

namespace WhatGameToPlay
{
    public class PlayerFile : TextFile, IFileCreator
    {
        private readonly PlayersDirectory _directory;

        public PlayerFile(string name, PlayersDirectory directory) : base(name)
        {
            _directory = directory;
            CreateFileIfNotExists();
        }

        public void CreateFileIfNotExists()
        {
            FilesCreator.CreateFileIfNotExists(_directory.GetSelectedFilePath(_name));
        }

        public void Delete()
            => File.Delete(_directory.GetSelectedFilePath(_name));
    }
}
