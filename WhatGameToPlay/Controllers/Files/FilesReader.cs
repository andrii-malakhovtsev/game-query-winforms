using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace WhatGameToPlay
{
    sealed public class FilesReader : FilesController
    {
        private const string TextFilesInDirectory = "*" + TextFileExtension;
        private static readonly char[] WindowsFilesBannedCharacters = "\\/:*?\"<>|".ToCharArray();

        public static FileInfo[] PlayersTextFiles { get => GetTextFiles(directoryName: PlayersDirectoryName); }

        public static FileInfo[] PlayersLimitsTextFiles { get => GetTextFiles(directoryName: LimitsDirectoryName); }

        public static bool StandartFilesExist { get => File.Exists(ThemeFileName); }

        public static string CurrentThemeFromFile { get => File.ReadAllLines(ThemeFileName)[0]; }

        public static string[] OptionsFromFile { get => File.ReadAllLines(OptionsFileName); }

        public static string[] GamesFromFile { get => File.ReadAllLines(GamesListFileName); }

        public static List<string> GamesListFromFile { get => File.ReadAllLines(GamesListFileName).OrderBy(game => game).ToList(); }

        public static List<Player> PlayersFromDirectory
        {
            get
            {
                var players = new List<Player>();
                foreach (FileInfo fileInfo in PlayersTextFiles)
                {
                    var gamesNotPlaying = File.ReadAllLines(fileInfo.FullName).ToList();
                    players.Add(new Player(Path.GetFileNameWithoutExtension(fileInfo.Name), gamesNotPlaying));
                }
                return players;
            }
        }

        public static bool StringContainsBannedSymbols(string @string)
        {
            foreach (char @char in WindowsFilesBannedCharacters)
            {
                if (@string.Contains(@char)) return true;
            }
            return !@string.Any(letter => char.IsLetterOrDigit(letter)) || string.IsNullOrEmpty(@string);
        }

        public static bool PlayerFileExist(string checkPlayer)
        {
            return TextFileExist(PlayersTextFiles, checkPlayer);
        }

        public static bool PlayersLimitsFileExist(string checkGame)
        {
            return TextFileExist(PlayersLimitsTextFiles, checkGame);
        }

        private static string GetFullDirectoryFilePath(string directory, string fileName)
        {
            return directory + "\\" + fileName + TextFileExtension;
        }

        public static string GetSelectedGamePlayersLimitsFilePath(string gameName)
        {
            return GetFullDirectoryFilePath(LimitsDirectoryName, gameName);
        }

        public static string GetSelectedPlayerFilePath(string selectedPlayer)
        {
            return GetFullDirectoryFilePath(PlayersDirectoryName, selectedPlayer);
        }

        public static bool GetPlayersLimitsFromGameFile(string gameName, out decimal[] limits)
        {
            const int limitsCount = 2;
            limits = new decimal[limitsCount];
            foreach (FileInfo file in PlayersLimitsTextFiles)
            {
                if (gameName == Path.GetFileNameWithoutExtension(file.Name))
                {
                    string[] fileRead = File.ReadAllLines(file.FullName);
                    for (int i = 0; i < limits.Length; i++)
                    {
                        limits[i] = Convert.ToDecimal(fileRead[i]);
                    }
                    return true;
                }
            }
            return false;
        }

        public static string[] GetGamesPlayerDoesNotPlay(string selectedPlayer)
        {
            foreach (FileInfo file in PlayersTextFiles)
            {
                if (selectedPlayer == Path.GetFileNameWithoutExtension(file.Name))
                    return File.ReadAllLines(file.FullName);
            }
            return null;
        }

        public static List<string> GetLimitedGamesFromDirectory(int checkedPlayersCount)
        {
            var limitedGames = new List<string>();
            foreach (FileInfo fileInfo in PlayersLimitsTextFiles)
            {
                string[] lines = File.ReadAllLines(fileInfo.FullName);
                bool playerCountOutsideLimits = checkedPlayersCount < Convert.ToInt32(lines[0]) ||
                    checkedPlayersCount > Convert.ToInt32(lines[1]);
                if (playerCountOutsideLimits)
                {
                    limitedGames.Add(Path.GetFileNameWithoutExtension(fileInfo.Name));
                }
            }
            return limitedGames;
        }

        private static bool TextFileExist(FileInfo[] filesCollection, string fileName)
        {
            foreach (FileInfo fileInfo in filesCollection)
                if (fileName == Path.GetFileNameWithoutExtension(fileInfo.Name)) return true;
            return false;
        }

        private static FileInfo[] GetTextFiles(string directoryName)
        {
            var directory = new DirectoryInfo(directoryName);
            return directory.GetFiles(TextFilesInDirectory);
        }
    }
}
