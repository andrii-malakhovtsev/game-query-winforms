using System;
using System.IO;
using System.Linq;

namespace WhatGameToPlay
{
    public class FilesController
    {
        protected const string TextFileExtension = ".txt";
        protected const string ThemeFileName = "Theme" + TextFileExtension;
        protected const string OptionsFileName = "Options" + TextFileExtension;
        protected const string GamesListFileName = "GamesList" + TextFileExtension;
        protected const string PlayersDirectoryName = "Players";
        protected const string LimitsDirectoryName = "Limits";

        public static void CreateStartingFiles()
        {
            if (CreateFile(ThemeFileName))
            {
                FilesWriter.WriteThemeToFile(ThemeController.StandartTheme);
            }
            if (CreateFile(OptionsFileName))
            {
                string[] standartOptions = { "True", "True", "True", "False", "True", "False" };
                FilesWriter.WriteOptionsToFile(standartOptions);
            }
            if (CreateFile(GamesListFileName))
            {
                FilesWriter.AddGameToGameListFile("example game");
            }
            if (CreateDirectory(PlayersDirectoryName))
            {
                CreatePlayerFile("example player");
            }
            CreateDirectory(LimitsDirectoryName);
        }

        protected static bool CreateFile(string fileName)
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
            CreateFile(fileName: FilesReader.GetSelectedPlayerFilePath(selectedPlayer));
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

        public static void DeleteGameFromGameList(string gameToDelete)
        {
            string[] games = FilesReader.GamesFromFile;
            File.WriteAllText(GamesListFileName, string.Empty);
            foreach (string game in games)
            {
                if (game != gameToDelete)
                {
                    FilesWriter.AddGameToGameListFile(game);
                }
            }
        }

        private static void DeleteGameFromPlayersFiles(string gameToDelete)
        {
            foreach (FileInfo fileInfo in FilesReader.PlayersTextFiles)
            {
                File.WriteAllLines(fileInfo.FullName,
                    File.ReadLines(fileInfo.FullName).Where(game => game != gameToDelete).ToList());
            }
        }

        public static void DeletePlayersLimitsFile(string gameName)
        {
            string path = FilesReader.GetSelectedGamePlayersLimitsFilePath(gameName);
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
            File.Delete(FilesReader.GetSelectedPlayerFilePath(selectedPlayer));
        }
    }
}
