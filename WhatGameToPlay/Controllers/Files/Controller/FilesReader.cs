using System.IO;
using System.Linq;

namespace WhatGameToPlay
{
    public class FilesReader
    {
        private readonly MainFormModel _mainFormModel;

        public FilesReader(MainFormModel mainFormModel) => _mainFormModel = mainFormModel;

        public bool StandartFilesExist => File.Exists(_mainFormModel.Files.Theme.Name);

        public static bool StringContainsBannedSymbols(string @string)
        {
            string WindowsFilesBannedCharacters = "\\/:*?\"<>|";
            foreach (char @char in WindowsFilesBannedCharacters)
            {
                if (@string.Contains(@char)) return true;
            }
            return !@string.Any(letter => char.IsLetterOrDigit(letter)) || string.IsNullOrEmpty(@string);
        }

        public static bool TextFileExist(FileInfo[] filesCollection, string fileName)
        {
            foreach (FileInfo fileInfo in filesCollection)
                if (fileName == Path.GetFileNameWithoutExtension(fileInfo.Name)) return true;
            return false;
        }
    }
}
