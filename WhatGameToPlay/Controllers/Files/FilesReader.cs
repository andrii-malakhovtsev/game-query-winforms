using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace WhatGameToPlay
{
    static public class FilesReader
    {
        private const string TextFilesInDirectory = "*" + FilesNames.TextFileExtension;
        private static readonly char[] WindowsFilesBannedCharacters = "\\/:*?\"<>|".ToCharArray();

        private static FileInfo[] PlayersLimitsTextFiles => GetTextFiles(directoryName: FilesNames.LimitsDirectoryName); 

        public static FileInfo[] PlayersTextFiles => GetTextFiles(directoryName: FilesNames.PlayersDirectoryName); 

        public static bool StandartFilesExist  => File.Exists(FilesNames.ThemeFileName);

        public static string CurrentThemeFromFile => File.ReadAllLines(FilesNames.ThemeFileName)[0];

        public static string[] OptionsFromFile => File.ReadAllLines(FilesNames.OptionsFileName);

        public static string[] GamesFromFile => File.ReadAllLines(FilesNames.GamesListFileName);

        public static List<string> GamesListFromFile => File.ReadAllLines(FilesNames.GamesListFileName).OrderBy(game => game).ToList();

        public static List<Player> PlayersFromDirectory
        {
            get
            {
                var players = new List<Player>();
                foreach (FileInfo playerTextFile in PlayersTextFiles)
                {
                    AddPlayerFromTextFile(players, playerTextFile);
                }
                return players;
            }
        }

        private static void AddPlayerFromTextFile(List<Player> players, FileInfo playerTextFile)
        {
            var gamesNotPlaying = File.ReadAllLines(playerTextFile.FullName).ToList();
            players.Add(new Player(Path.GetFileNameWithoutExtension(playerTextFile.Name), gamesNotPlaying));
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
            => TextFileExist(PlayersTextFiles, checkPlayer);

        public static bool PlayersLimitsFileExist(string checkGame) 
            => TextFileExist(PlayersLimitsTextFiles, checkGame);

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
            foreach (FileInfo file in PlayersLimitsTextFiles)
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
            foreach (FileInfo playerLimitsTextFileInfo in PlayersLimitsTextFiles)
            {
                AddLimitedGamesFromSpecificPlayer(limitedGames, playerLimitsTextFileInfo, checkedPlayersCount);
            }
            return limitedGames;
        }

        private static void AddLimitedGamesFromSpecificPlayer(List<string> limitedGames, FileInfo playerLimitsTextFileInfo, 
            int checkedPlayersCount)
        {
            string[] lines = File.ReadAllLines(playerLimitsTextFileInfo.FullName);
            bool playerCountOutsideLimits =
                checkedPlayersCount < Convert.ToInt32(lines[0]) ||
                checkedPlayersCount > Convert.ToInt32(lines[1]);
            if (playerCountOutsideLimits)
            {
                limitedGames.Add(Path.GetFileNameWithoutExtension(playerLimitsTextFileInfo.Name));
            }
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
