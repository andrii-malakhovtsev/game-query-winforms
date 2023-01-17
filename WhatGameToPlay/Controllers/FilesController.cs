using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace WhatGameToPlay
{
    public static class FilesController
    {
        private const string TextFileExtension = ".txt";
        private const string AllTextFilesExtension = "*" + TextFileExtension;
        private const string ThemeFileName = "Theme" + TextFileExtension;
        private const string OptionsFileName = "Options" + TextFileExtension;
        private const string GamesListFileName = "GamesList" + TextFileExtension;
        private const string PlayersDirectoryName = "Players";
        private const string LimitsDirectoryName = "Limits";
        private static readonly char[] s_charactersFilesCanNotUse = "\\/:*?\"<>|".ToCharArray();

        public static bool StandartFilesExist()
        {
            return File.Exists(ThemeFileName);
        }

        public static bool StringContainsBannedSymbols(string @string)
        {
            foreach (char @char in s_charactersFilesCanNotUse){
                if (@string.Contains(@char)) return true;
            }
            return !@string.Any(letter => char.IsLetterOrDigit(letter)) || string.IsNullOrEmpty(@string);
        }

        public static bool PlayerFileExist(string checkPlayer)
        {
            return TextFileExist(GetPlayersTextFiles(), checkPlayer);
        }

        public static bool PlayersLimitsFileExist(string checkGame)
        {
            return TextFileExist(GetPlayersLimitsTextFiles(), checkGame);
        }

        private static bool TextFileExist(FileInfo[] filesCollection, string fileName)
        {
            foreach (FileInfo fileInfo in filesCollection)
                if (fileName == Path.GetFileNameWithoutExtension(fileInfo.Name)) return true;
            return false;
        }

        public static void CreateStartingFiles()
        {
            if (CreateFile(ThemeFileName))
            {
                WriteThemeToFile(ThemeController.GetStandartThemeName());
            }
            if (CreateFile(OptionsFileName))
            {
                string[] standartOptions = { "True", "True", "True", "False", "True", "False" };
                WriteOptionsToFile(standartOptions);
            }
            if (CreateFile(GamesListFileName))
            {
                AddGameToGameListFile("example game");
            }
            if (CreateDirectory(PlayersDirectoryName))
            {
                CreatePlayerFile("example player");
            }
            CreateDirectory(LimitsDirectoryName);
        }

        private static bool CreateFile(string fileName)
        {
            bool fileDoesNotExist = !File.Exists(fileName);
            if (fileDoesNotExist)
            {
                File.Create(fileName).Dispose();
            }
            return fileDoesNotExist;
        }

        public static void CreatePlayerFile(string selectedPlayer)
        {
            CreateFile(fileName: GetSelectedPlayerFilePath(selectedPlayer));
        }

        private static bool CreateDirectory(string directoryName)
        {
            if (!Directory.Exists(directoryName))
            {
                Directory.CreateDirectory(directoryName);
                return true;
            }
            return false;
        }

        private static string GetFullDirectoryFilePath(string directory, string fileName)
        {
            return directory + "\\" + fileName + TextFileExtension;
        }

        public static string GetCurrentTheme()
        {
            string[] fileRead = File.ReadAllLines(ThemeFileName);
            return fileRead[0];
        }

        private static string GetSelectedGamePlayersLimitsFilePath(string gameName)
        {
            return GetFullDirectoryFilePath(LimitsDirectoryName, gameName);
        }

        private static string GetSelectedPlayerFilePath(string selectedPlayer)
        {
            return GetFullDirectoryFilePath(PlayersDirectoryName, selectedPlayer);
        }

        public static bool GetPlayersLimitsFromGameFile(string gameName,
            ref decimal[] limits)
        {
            foreach (FileInfo file in GetPlayersLimitsTextFiles())
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

        public static string[] GetOptionsFromFile()
        {
            return File.ReadAllLines(OptionsFileName);
        }

        public static string[] GetGamesFromFile()
        {
            return File.ReadAllLines(GamesListFileName);
        }

        public static string[] GetGamesPlayerDoesntPlay(string selectedPlayer)
        {
            foreach (FileInfo file in GetPlayersTextFiles())
            {
                if (selectedPlayer == Path.GetFileNameWithoutExtension(file.Name))
                    return File.ReadAllLines(file.FullName);
            }
            return null;
        }
        
        private static FileInfo[] GetTextFiles(string directoryName)
        {
            DirectoryInfo directory = new DirectoryInfo(directoryName);
            return directory.GetFiles(AllTextFilesExtension);
        }

        private static FileInfo[] GetPlayersTextFiles()
        {
            return GetTextFiles(directoryName: PlayersDirectoryName);
        }

        private static FileInfo[] GetPlayersLimitsTextFiles()
        {
            return GetTextFiles(directoryName: LimitsDirectoryName);
        }

        public static List<string> GetLimitedGamesFromDirectory(int checkedPlayersCount)
        {
            List<string> limitedGames = new List<string>();
            foreach (FileInfo fileInfo in GetPlayersLimitsTextFiles())
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

        public static List<string> GetGamesListFromFile()
        {
            List<string> games = new List<string>();
            foreach (string game in File.ReadAllLines(GamesListFileName))
            {
                games.Add(game);
            }
            games.Sort();
            return games;
        }

        public static List<Player> GetPlayersListFromDirectory()
        {
            List<Player> players = new List<Player>();
            foreach (FileInfo fileInfo in GetPlayersTextFiles())
            {
                List<string> gamesNotPlaying = new List<string>();
                foreach (string gameDoesNotPlay in File.ReadAllLines(fileInfo.FullName))
                {
                    gamesNotPlaying.Add(gameDoesNotPlay);
                }
                players.Add(new Player(Path.GetFileNameWithoutExtension(fileInfo.Name), gamesNotPlaying));
            }
            return players;
        }

        public static void AppendGameToPlayersFiles(string gameName)
        {
            foreach (FileInfo file in GetPlayersTextFiles())
            {
                File.AppendAllText(file.FullName, gameName + "\n");
            }
        }

        public static void AddGameToGameListFile(string gameName)
        {
            File.AppendAllText(GamesListFileName, gameName + Environment.NewLine);
        }

        public static void WriteThemeToFile(string theme)
        {
            File.WriteAllText(ThemeFileName, theme);
        }

        public static void WriteOptionsToFile(string[] options)
        {
            File.WriteAllLines(OptionsFileName, options);
        }

        public static void WritePlayersLimitsToFile(string gameName, decimal minValue, decimal maxValue)
        {
            string path = GetSelectedGamePlayersLimitsFilePath(gameName);
            CreateFile(path);
            using (TextWriter textWriter = new StreamWriter(path))
            {
                textWriter.WriteLine(Convert.ToString(minValue));
                textWriter.WriteLine(Convert.ToString(maxValue));
            }
        }
        
        public static void WriteGamesNotPlayingToFile(string selectedPlayer,
            List<string> gamesNotPlayingList)
        {
            string path = GetSelectedPlayerFilePath(selectedPlayer);
            CreatePlayerFile(selectedPlayer);
            using (TextWriter textWriter = new StreamWriter(path))
            {
                foreach (string gameNotPlaying in gamesNotPlayingList)
                {
                    textWriter.WriteLine(gameNotPlaying.ToString());
                }
            }
        }

        public static void DeleteGameFromGameList(string gameToDelete)
        {
            string[] games = GetGamesFromFile();
            File.WriteAllText(GamesListFileName, string.Empty);
            foreach (string game in games)
            {
                if (game != gameToDelete)
                {
                    AddGameToGameListFile(game);
                }
            }
        }

        private static void DeleteGameFromPlayersFiles(string gameToDelete)
        {
            foreach (FileInfo fileInfo in GetPlayersTextFiles())
            {
                File.WriteAllLines(fileInfo.FullName,
                    File.ReadLines(fileInfo.FullName).Where(game => game != gameToDelete).ToList());
            }
        }

        public static void DeletePlayersLimitsFile(string gameName)
        {
            string path = GetSelectedGamePlayersLimitsFilePath(gameName);
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }

        public static void DeletePlayersGameData(string gameToDelete)
        {
            DeletePlayersLimitsFile(gameName: gameToDelete);
            DeleteGameFromPlayersFiles(gameToDelete: gameToDelete);
        }

        public static void DeleteSelectedPlayerFile(string selectedPlayer)
        {
            File.Delete(GetSelectedPlayerFilePath(selectedPlayer));
        }
    }
}
