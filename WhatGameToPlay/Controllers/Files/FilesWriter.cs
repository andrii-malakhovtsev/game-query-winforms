using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace WhatGameToPlay
{
    sealed public class FilesWriter : FilesController
    {
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
            string path = FilesReader.GetSelectedGamePlayersLimitsFilePath(gameName);
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
            string path = FilesReader.GetSelectedPlayerFilePath(selectedPlayer);
            CreatePlayerFile(selectedPlayer);
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
        {
            File.AppendAllText(GamesListFileName, gameName + Environment.NewLine);
        }
    }
}
