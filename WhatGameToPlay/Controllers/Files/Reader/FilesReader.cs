using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace WhatGameToPlay
{
    public static class FilesReader
    {
        public static bool StandartFilesExist  => File.Exists(FilesNames.ThemeFileName);

        public static string CurrentThemeFromFile => File.ReadAllLines(FilesNames.ThemeFileName)[0];

        public static string[] OptionsFromFile => File.ReadAllLines(FilesNames.OptionsFileName);

        public static string[] GamesFromFile => File.ReadAllLines(FilesNames.GamesListFileName);

        public static List<string> GamesListFromFile 
            => File.ReadAllLines(FilesNames.GamesListFileName).OrderBy(game => game).ToList();

        public static bool StringContainsBannedSymbols(string @string)
        {
            string WindowsFilesBannedCharacters = "\\/:*?\"<>|";
            foreach (char @char in WindowsFilesBannedCharacters)
            {
                if (@string.Contains(@char)) return true;
            }
            return !@string.Any(letter => char.IsLetterOrDigit(letter)) || string.IsNullOrEmpty(@string);
        }

        public static bool PlayerFileExist(string checkPlayer) 
            => TextFileExist(DirectoryReader.PlayersTextFiles, checkPlayer);

        public static bool PlayersLimitsFileExist(string checkGame) 
            => TextFileExist(DirectoryReader.PlayersLimitsTextFiles, checkGame);

        private static bool TextFileExist(FileInfo[] filesCollection, string fileName)
        {
            foreach (FileInfo fileInfo in filesCollection)
                if (fileName == Path.GetFileNameWithoutExtension(fileInfo.Name)) return true;
            return false;
        }

        private static string GetFullDirectoryFilePath(string directory, string fileName) 
            => directory + "\\" + fileName + FilesNames.TextFileExtension;

        public static string GetSelectedGamePlayersLimitsFilePath(string gameName)
            => GetFullDirectoryFilePath(FilesNames.LimitsDirectoryName, gameName);

        public static string GetSelectedPlayerFilePath(string selectedPlayer)
            => GetFullDirectoryFilePath(FilesNames.PlayersDirectoryName, selectedPlayer);

        public static bool GetPlayersLimitsFromGameFile(string gameName, out decimal[] limits)
        {
            const int limitsCount = 2;
            limits = new decimal[limitsCount];

            foreach (FileInfo file in DirectoryReader.PlayersLimitsTextFiles)
            {
                if (gameName == Path.GetFileNameWithoutExtension(file.Name))
                {
                    SetLimitsFromSpecificFile(limits, file);
                    return true;
                }
            }
            return false;
        }

        private static void SetLimitsFromSpecificFile(decimal[] limits, FileInfo file)
        {
            string[] fileRead = File.ReadAllLines(file.FullName);
            for (int i = 0; i < limits.Length; i++)
            {
                limits[i] = Convert.ToDecimal(fileRead[i]);
            }
        }
    }
}
