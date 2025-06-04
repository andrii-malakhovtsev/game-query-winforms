using System.IO;
using System.Linq;

namespace WhatGameToPlay
{
    public class FilesReader
    {
        private readonly MainFormModel _mainFormModel;

        public FilesReader(MainFormModel mainFormModel) => _mainFormModel = mainFormModel;

        public bool StandardFilesExist => File.Exists(_mainFormModel.Files.Theme.Name);

        public static bool NameContainsBannedSymbols(string fileName)
        {
            const string WindowsBannedChars = "\\/:*?\"<>|";
            foreach (char bannedChar in WindowsBannedChars)
            {
                if (fileName.Contains(bannedChar)) return true;
            }
            return !fileName.Any(letter => char.IsLetterOrDigit(letter)) || string.IsNullOrEmpty(fileName);
            // double test this function
        }

        public static bool TextFileExist(FileInfo[] filesCollection, string fileName)
        {
            foreach (FileInfo fileInfo in filesCollection)
                if (fileName == Path.GetFileNameWithoutExtension(fileInfo.Name)) return true;
            return false;
        }
    }
}
