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
                WriteToFile(Theme.Standart.Name); // mb change later after check to action?
        }

        public string CurrentTheme => File.ReadAllLines(_name)[0];

        public void WriteToFile(params string[] theme) => File.WriteAllText(_name, theme[0]);
    }
}
