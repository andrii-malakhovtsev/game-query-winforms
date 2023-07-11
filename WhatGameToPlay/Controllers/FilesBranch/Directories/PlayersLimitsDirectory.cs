using System;
using System.Collections.Generic;
using System.IO;

namespace WhatGameToPlay
{
    public class PlayersLimitsDirectory : Directory
    {
        public PlayersLimitsDirectory(string name) : base(name) { }

        public override FileInfo[] TextFiles
            => GetTextFiles(directoryName: _name); // mb I can get rid of "Name" here and actually in many other places

        private string GetSelectedGamePlayersLimitsFilePath(string gameName) // to interface
            => GetFullDirectoryFilePath(_name, gameName);

        public bool PlayersLimitsFileExist(string checkGame)
            => FilesReader.TextFileExist(TextFiles, checkGame);

        public override void CreateDirectoryIfNotExists()
            => FilesCreator.CreateDirectoryIfNotExists(_name);

        public List<string> GetLimitedGamesList(int checkedPlayersCount)
        {
            var limitedGames = new List<string>();
            foreach (FileInfo playerLimitsTextFileInfo in TextFiles)
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

        public bool GetPlayersLimits(string gameName, out decimal[] limits)
        {
            const int limitsCount = 2;
            limits = new decimal[limitsCount];

            foreach (FileInfo file in TextFiles)
            {
                if (gameName == Path.GetFileNameWithoutExtension(file.Name))
                {
                    SetLimitsFromSpecificFile(limits, file);
                    return true;
                }
            }
            return false;
        }

        private static void SetLimitsFromSpecificFile(decimal[] limits, FileInfo file)
        {
            string[] fileRead = File.ReadAllLines(file.FullName);
            for (int i = 0; i < limits.Length; i++)
            {
                limits[i] = Convert.ToDecimal(fileRead[i]);
            }
        }

        public void WriteToFiles(string gameName, decimal minValue, decimal maxValue)
        {
            string path = GetSelectedGamePlayersLimitsFilePath(gameName);
            FilesCreator.CreateFileIfNotExists(path);
            using (TextWriter textWriter = new StreamWriter(path))
            {
                textWriter.WriteLine(Convert.ToString(minValue));
                textWriter.WriteLine(Convert.ToString(maxValue));
            }
        }

        public void DeletePlayersLimitsFile(string gameName)
        {
            string path = GetFullDirectoryFilePath(directory: _name, gameName);
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
    }
}
