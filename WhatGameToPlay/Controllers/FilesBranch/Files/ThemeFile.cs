using System.IO;

namespace WhatGameToPlay
{
    public class ThemeFile : TextFile, IFileCreator, IFileWriter
    {
        public ThemeFile(string name) : base(name) {}

        public string Name => _name;

        public void CreateFileIfNotExists()
        {
            if (FilesCreator.CreateFileIfNotExists(_name))
                WriteToFile(Theme.Standart.Name);
        }

        public void WriteToFile(params string[] theme) => File.WriteAllText(_name, theme[0]);

        public string CurrentThemeName => File.ReadAllLines(_name)[0];
    }
}
