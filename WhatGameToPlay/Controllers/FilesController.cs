using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace WhatGameToPlay
{
    public static class FilesController // add FileReader/FileWriter classes
    {
        private const string TextFileExtension = ".txt";
        private const string AllTextFilesExtension = "*" + TextFileExtension;
        private const string ThemeFileName = "Theme" + TextFileExtension;
        private const string OptionsFileName = "Options" + TextFileExtension;
        private const string GamesListFileName = "GamesList" + TextFileExtension;
        private const string PlayersDirectoryName = "Players";
        private const string LimitsDirectoryName = "Limits";
        private static readonly char[] s_charactersFilesCanNotUse = "\\/:*?\"<>|".ToCharArray();

        private static FileInfo[] PlayersTextFiles { get => GetTextFiles(directoryName: PlayersDirectoryName); }
        private static FileInfo[] PlayersLimitsTextFiles { get => GetTextFiles(directoryName: LimitsDirectoryName); }
        public static bool StandartFilesExist { get => File.Exists(ThemeFileName); }
        public static string CurrentThemeFromFile
        {
            get
            {
                string[] fileRead = File.ReadAllLines(ThemeFileName);
                return fileRead[0];
            }
        }
        public static string[] OptionsFromFile { get => File.ReadAllLines(OptionsFileName); }
        public static string[] GamesFromFile { get => File.ReadAllLines(GamesListFileName); }
        public static List<string> GamesListFromFile
        {
            get
            {
                var games = new List<string>();
                foreach (string game in File.ReadAllLines(GamesListFileName))
                {
                    games.Add(game);
                }
                games.Sort();
                return games;
            }
        }
        public static List<Player> PlayersFromDirectory
        {
            get
            {
                var players = new List<Player>();
                foreach (FileInfo fileInfo in PlayersTextFiles)
                {
                    var gamesNotPlaying = new List<string>();
                    foreach (string gameDoesNotPlay in File.ReadAllLines(fileInfo.FullName))
                    {
                        gamesNotPlaying.Add(gameDoesNotPlay);
                    }
                    players.Add(new Player(Path.GetFileNameWithoutExtension(fileInfo.Name), gamesNotPlaying));
                }
                return players;
            }
        }

        public static bool StringContainsBannedSymbols(string @string)
        {
            foreach (char @char in s_charactersFilesCanNotUse)
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
                WriteThemeToFile(ThemeController.StandartTheme);
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

        public static string[] GetGamesPlayerDoesntPlay(string selectedPlayer)
        {
            foreach (FileInfo file in PlayersTextFiles)
            {
                if (selectedPlayer == Path.GetFileNameWithoutExtension(file.Name))
                    return File.ReadAllLines(file.FullName);
            }
            return null;
        }

        private static FileInfo[] GetTextFiles(string directoryName)
        {
            var directory = new DirectoryInfo(directoryName);
            return directory.GetFiles(AllTextFilesExtension);
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

        public static List<Player> GetPlayersFromDirectory()
        {
            var players = new List<Player>();
            foreach (FileInfo fileInfo in PlayersTextFiles)
            {
                var gamesNotPlaying = new List<string>();
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
            foreach (FileInfo file in PlayersTextFiles)
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
            string[] games = GamesFromFile;
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
            foreach (FileInfo fileInfo in PlayersTextFiles)
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
