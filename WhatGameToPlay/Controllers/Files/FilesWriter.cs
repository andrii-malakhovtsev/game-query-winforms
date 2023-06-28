using System;
using System.Collections.Generic;
using System.IO;

namespace WhatGameToPlay
{
    static public class FilesWriter
    {
        public static void WriteThemeToFile(string theme) => File.WriteAllText(FilesNames.ThemeFileName, theme); 
        // polymorphism can be used

        public static void WriteOptionsToFile(string[] options) => File.WriteAllLines(FilesNames.OptionsFileName, options);

        public static void WritePlayersLimitsToFile(string gameName, decimal minValue, decimal maxValue)
        {
            string path = FilesReader.GetSelectedGamePlayersLimitsFilePath(gameName);
            FilesController.CreateFile(path);
            using (TextWriter textWriter = new StreamWriter(path))
            {
                textWriter.WriteLine(Convert.ToString(minValue));
                textWriter.WriteLine(Convert.ToString(maxValue));
            }
        }

        public static void WriteGamesNotPlayingToFile(string selectedPlayer,
            List<string> gamesNotPlayingList)
        {
            string path = FilesReader.GetSelectedPlayerFilePath(selectedPlayer);
            FilesController.CreatePlayerFile(selectedPlayer);
            using (TextWriter textWriter = new StreamWriter(path))
            {
                foreach (string gameNotPlaying in gamesNotPlayingList)
                {
                    textWriter.WriteLine(gameNotPlaying.ToString());
                }
            }
        }

        public static void AppendGameToPlayersFiles(string gameName)
        {
            foreach (FileInfo file in FilesReader.PlayersTextFiles)
            {
                File.AppendAllText(file.FullName, gameName + "\n");
            }
        }

        public static void AddGameToGameListFile(string gameName)
            => File.AppendAllText(FilesNames.GamesListFileName, gameName + Environment.NewLine);
    }
}
