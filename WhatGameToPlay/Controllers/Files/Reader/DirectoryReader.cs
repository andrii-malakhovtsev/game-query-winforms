using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace WhatGameToPlay
{
    public class DirectoryReader
    {
        private const string TextFilesInDirectory = "*" + FilesNames.TextFileExtension;

        public static FileInfo[] PlayersLimitsTextFiles
            => GetTextFiles(directoryName: FilesNames.LimitsDirectoryName);

        public static FileInfo[] PlayersTextFiles
            => GetTextFiles(directoryName: FilesNames.PlayersDirectoryName);

        private static FileInfo[] GetTextFiles(string directoryName)
        {
            var directory = new DirectoryInfo(directoryName);
            return directory.GetFiles(TextFilesInDirectory);
        }

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
            foreach (FileInfo playerLimitsTextFileInfo in DirectoryReader.PlayersLimitsTextFiles)
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
    }
}
