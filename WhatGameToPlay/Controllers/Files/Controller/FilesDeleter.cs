using System.IO;
using System.Linq;

namespace WhatGameToPlay
{
    public class FilesDeleter
    {
        public static void DeleteGameFromGameList(string gameToDelete)
        {
            File.WriteAllText(FilesNames.GamesListFileName, string.Empty);

            string[] gamesExceptGameToDelete = FilesReader.GamesFromFile.Where(game => game != gameToDelete).ToArray();
            foreach (string game in gamesExceptGameToDelete)
                FilesWriter.AddGameToGameListFile(game);
        }

        private static void DeleteGameFromPlayersFiles(string gameToDelete)
        {
            foreach (FileInfo fileInfo in DirectoryReader.PlayersTextFiles)
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
