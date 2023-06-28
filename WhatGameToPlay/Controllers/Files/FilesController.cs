using System.IO;
using System.Linq;

namespace WhatGameToPlay
{
    static public class FilesController
    {
        public static void CreateStartingFiles()
        {
            if (CreateFile(FilesNames.ThemeFileName))
            {
                FilesWriter.WriteThemeToFile(Theme.Standart.Name);
            }
            if (CreateFile(FilesNames.OptionsFileName))
            {
                string[] standartOptions = { "True", "True", "True", "False", "True", "False" };
                FilesWriter.WriteOptionsToFile(standartOptions);
            }
            if (CreateFile(FilesNames.GamesListFileName))
            {
                FilesWriter.AddGameToGameListFile("example game");
            }
            if (CreateDirectory(FilesNames.PlayersDirectoryName))
            {
                CreatePlayerFile("example player");
            }
            CreateDirectory(FilesNames.LimitsDirectoryName);
        }

        public static void CreatePlayerFile(string selectedPlayer) 
            => CreateFile(fileName: FilesReader.GetSelectedPlayerFilePath(selectedPlayer));

        public static bool CreateFile(string fileName)
        {
            bool fileDoesNotExist = !File.Exists(fileName);
            if (fileDoesNotExist)
            {
                File.Create(fileName).Dispose();
            }
            return fileDoesNotExist;
        }

        private static bool CreateDirectory(string directoryName)
        {
            bool directoryDoesNotExist = !Directory.Exists(directoryName);
            if (directoryDoesNotExist)
            {
                Directory.CreateDirectory(directoryName);
            }
            return directoryDoesNotExist;
        }

        public static void DeleteGameFromGameList(string gameToDelete)
        {
            string[] gamesExceptGameToDelete = FilesReader.GamesFromFile.Where(game => game != gameToDelete).ToArray();
            File.WriteAllText(FilesNames.GamesListFileName, string.Empty);
            foreach (string game in gamesExceptGameToDelete)
                FilesWriter.AddGameToGameListFile(game);
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
            => File.Delete(FilesReader.GetSelectedPlayerFilePath(selectedPlayer));
    }
}
