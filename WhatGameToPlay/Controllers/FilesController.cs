using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace WhatGameToPlay
{
    public static class FilesController
    {
        public static string GetCurrentTheme()
        {
            string[] fileRead = File.ReadAllLines("Theme.txt");
            return fileRead[0];
        }

        private static FileInfo[] GetRestrictionsTextFiles()
        {
            DirectoryInfo directory = new DirectoryInfo("Restrictions");
            return directory.GetFiles("*.txt");
        }

        private static FileInfo[] GetPlayersTextFiles()
        {
            DirectoryInfo directory = new DirectoryInfo("Players");
            return directory.GetFiles("*.txt");
        }

        public static void AppendGameToPlayerFile(string gameName)
        {
            foreach (FileInfo file in GetPlayersTextFiles())
                File.AppendAllText(file.FullName, gameName + "\n");
        }

        public static void AddGameToGameListFile(string gameName)
        {
            File.AppendAllText("GamesList.txt", gameName + Environment.NewLine);
        }

        public static void RestrictionsToFile(string gameName, decimal minValue, decimal maxValue)
        {
            string restrictionsFilePath = "Restrictions\\" + gameName + ".txt";
            if (!File.Exists(restrictionsFilePath)) File.Create(restrictionsFilePath).Dispose();
            using (TextWriter textWriter = new StreamWriter(restrictionsFilePath))
            {
                textWriter.WriteLine(Convert.ToString(minValue));
                textWriter.WriteLine(Convert.ToString(maxValue));
            }
        }

        public static void DeleteGameFromFile(string gameName)
        {
            foreach (FileInfo fileInfo in GetPlayersTextFiles())
            {
                File.WriteAllLines(fileInfo.FullName,
                    File.ReadLines(fileInfo.FullName).Where(game => game != gameName).ToList());
            }
            string path = "Restrictions\\" + gameName + ".txt";
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

        public static bool RestrictionExist(string gameName, ref string gameFullName)
        {
            foreach (FileInfo fileInfo in GetRestrictionsTextFiles())
            {
                if (gameName == Path.GetFileNameWithoutExtension(fileInfo.Name))
                {
                    gameFullName = fileInfo.Name;
                    return true;
                }
            }
            return false;
        }

        public static void DeleteFile(string fileName)
        {
            File.Delete(fileName);
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
            string path = "Players\\" + selectedPlayer + ".txt";
            if (!File.Exists(path)) File.Create(path).Dispose();
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

        public static void CreatePersonFile(string selectedPerson)
        {
            string path = "Players\\" + selectedPerson + ".txt";
            File.Create(path).Dispose();
        }
    }
}
