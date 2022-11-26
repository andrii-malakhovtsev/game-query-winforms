using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace WhatGameToPlay
{
    public static class FilesController
    {
        private static readonly string s_textFileExtension = ".txt";
        private static readonly string s_allTextFileExtension = "*" + s_textFileExtension;
        private static readonly string s_themeFileName = "Theme" + s_textFileExtension;
        private static readonly string s_optionsFileName = "Options" + s_textFileExtension;
        private static readonly string s_gamesListFileName = "GamesList" + s_textFileExtension;
        private static readonly string s_playersDirectoryName = "Players";
        private static readonly string s_limitsDirectoryName = "Limits";

        public static void CreateStartingFiles()
        {
            if (CreateFile(s_themeFileName))
                AddThemeToFile(ThemeController.GetStandartThemeName());
            if (CreateFile(s_optionsFileName))
            {
                string[] standartOptions = { "True", "True", "True", "False", "True", "False" };
                AddOptionsToFile(standartOptions);
            }
            if (CreateFile(s_gamesListFileName))
                AddGameToGameListFile("example game");
            if (CreateDirectory(s_playersDirectoryName))
                CreatePersonFile("example player");
            CreateDirectory(s_limitsDirectoryName);
        }

        public static bool StandartFilesExist()
        {
            return File.Exists(s_themeFileName);
        }

        private static bool CreateFile(string fileName)
        {
            if (!File.Exists(fileName))
            {
                File.Create(fileName).Dispose();
                return true;
            }
            return false;
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

        public static string GetCurrentTheme()
        {
            string[] fileRead = File.ReadAllLines(s_themeFileName);
            return fileRead[0];
        }

        public static string[] GetOptionsFromFile()
        {
            return File.ReadAllLines(s_optionsFileName);
        }

        public static string[] GetGamesFromFile()
        {
            return File.ReadAllLines(s_gamesListFileName);
        }

        private static FileInfo[] GetRestrictionsTextFiles()
        {
            DirectoryInfo directory = new DirectoryInfo(s_limitsDirectoryName);
            return directory.GetFiles(s_allTextFileExtension);
        }

        private static FileInfo[] GetPlayersTextFiles()
        {
            DirectoryInfo directory = new DirectoryInfo(s_playersDirectoryName);
            return directory.GetFiles(s_allTextFileExtension);
        }

        public static void AppendGameToPlayersFiles(string gameName)
        {
            foreach (FileInfo file in GetPlayersTextFiles())
                File.AppendAllText(file.FullName, gameName + "\n");
        }

        private static void DeleteGameFromPlayersFiles(string gameToDelete)
        {
            foreach (FileInfo fileInfo in GetPlayersTextFiles())
            {
                File.WriteAllLines(fileInfo.FullName,
                    File.ReadLines(fileInfo.FullName).Where(game => game != gameToDelete).ToList());
            }
        }

        public static void DeletePlayersGameData(string gameToDelete)
        {
            DeleteGameRestrictionsFile(gameToDelete);
            DeleteGameFromPlayersFiles(gameToDelete);
        }

        public static void DeleteGameFromGameList(string gameToDelete)
        {
            string[] games = GetGamesFromFile();
            File.WriteAllText(s_gamesListFileName, string.Empty);
            foreach (string game in games)
                if (game != gameToDelete) AddGameToGameListFile(game);
        }

        public static void AddThemeToFile(string theme)
        {
            File.WriteAllText(s_themeFileName, theme);
        }

        public static void AddOptionsToFile(string[] options)
        {
            File.WriteAllLines(s_optionsFileName, options);
        }

        public static void AddGameToGameListFile(string gameName)
        {
            File.AppendAllText(s_gamesListFileName, gameName + Environment.NewLine);
        }

        public static void AddRestrictionsToFile(string gameName, decimal minValue, decimal maxValue)
        {
            string path = GetSelectedGameRestrictionsFilePath(gameName);
            CreateFile(path);
            using (TextWriter textWriter = new StreamWriter(path))
            {
                textWriter.WriteLine(Convert.ToString(minValue));
                textWriter.WriteLine(Convert.ToString(maxValue));
            }
        }

        public static void DeleteGameRestrictionsFile(string gameName)
        {
            string path = GetSelectedGameRestrictionsFilePath(gameName);
            if (File.Exists(path)) File.Delete(path);
        }

        public static bool GetRestrictionsFromGameFile(string gameName,
            ref decimal[] numericUpDownsValues)
        {
            foreach (FileInfo file in GetRestrictionsTextFiles())
            {
                if (gameName == Path.GetFileNameWithoutExtension(file.Name))
                {
                    string[] fileRead = File.ReadAllLines(file.FullName);
                    for (int i = 0; i < numericUpDownsValues.Length; i++)
                        numericUpDownsValues[i] = Convert.ToDecimal(fileRead[i]);
                    return true;
                }
            }
            return false;
        }

        public static List<string> GetRestrictedGamesFromDirectory(int checkedPeopleCount)
        {
            List<string> restrictedGames = new List<string>();
            foreach (FileInfo fileInfo in GetRestrictionsTextFiles())
            {
                string[] lines = File.ReadAllLines(fileInfo.FullName);
                if (checkedPeopleCount < Convert.ToInt32(lines[0]) ||
                    checkedPeopleCount > Convert.ToInt32(lines[1]))
                    restrictedGames.Add(Path.GetFileNameWithoutExtension(fileInfo.Name));
            }
            return restrictedGames;
        }

        public static bool RestrictionExist(string gameName)
        {
            foreach (FileInfo fileInfo in GetRestrictionsTextFiles())
            {
                if (gameName == Path.GetFileNameWithoutExtension(fileInfo.Name)) return true;
            }
            return false;
        }

        public static void DeleteFile(string fileName)
        {
            File.Delete(fileName);
        }

        public static void DeleteSelectedPlayerFile(string selectedPlayer)
        {
            DeleteFile(GetSelectedPersonFilePath(selectedPlayer));
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

        public static void WriteGamesNotPlayingToFile(string selectedPlayer,
            List<string> gamesNotPlayingList)
        {
            string path = GetSelectedPersonFilePath(selectedPlayer);
            CreatePersonFile(selectedPlayer);
            using (TextWriter textWriter = new StreamWriter(path))
            {
                foreach (string gameNotPlaying in gamesNotPlayingList)
                    textWriter.WriteLine(gameNotPlaying.ToString());
            }
        }

        public static bool DoesPersonFileExist(string checkPerson)
        {
            foreach (FileInfo fileInfo in GetPlayersTextFiles())
                if (checkPerson == Path.GetFileNameWithoutExtension(fileInfo.Name)) return true;
            return false;
        }

        public static void CreatePersonFile(string selectedPlayer)
        {
            CreateFile(fileName: GetSelectedPersonFilePath(selectedPlayer));
        }

        private static string GetSelectedPersonFilePath(string selectedPlayer)
        {
            return GetFullDirectoryFilePath(s_playersDirectoryName, selectedPlayer);
        }

        private static string GetSelectedGameRestrictionsFilePath(string gameName)
        {
            return GetFullDirectoryFilePath(s_limitsDirectoryName, gameName);
        }

        private static string GetFullDirectoryFilePath(string directory, string fileName)
        {
            return directory + "\\" + fileName + s_textFileExtension;
        }

        public static List<string> GetGamesListFromFile()
        {
            List<string> games = new List<string>();
            foreach (string game in File.ReadAllLines(s_gamesListFileName))
                games.Add(game);
            return games;
        }

        public static List<Player> GetPlayersListFromDirectory()
        {
            List<Player> players = new List<Player>();
            foreach (FileInfo fileInfo in GetPlayersTextFiles())
            {
                List<string> gamesNotPlaying = new List<string>();
                foreach (string gameDoesntPlay in File.ReadAllLines(fileInfo.FullName))
                    gamesNotPlaying.Add(gameDoesntPlay);
                players.Add(new Player(Path.GetFileNameWithoutExtension(fileInfo.Name), gamesNotPlaying));
            }
            return players;
        }
    }
}
