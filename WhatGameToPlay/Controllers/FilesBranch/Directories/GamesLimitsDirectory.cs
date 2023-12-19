using System;
using System.Collections.Generic;
using System.IO;

namespace WhatGameToPlay
{
    public class GamesLimitsDirectory : Directory
    {
        public GamesLimitsDirectory(string name) : base(name) { }

        public override void CreateDirectoryIfNotExists() => FilesCreator.CreateDirectoryIfNotExists(_name);

        private string GetFilePath(string gameName) => GetFullDirectoryFilePath(gameName);

        public List<string> GetLimitedGamesList(int checkedPlayersCount)
        {
            var limitedGames = new List<string>();
            foreach (FileInfo gamesLimitsTextFileInfo in TextFiles)
            {
                AddLimitedGamesFromLimitFile(limitedGames, gamesLimitsTextFileInfo, checkedPlayersCount);
            }
            return limitedGames;
        }

        private static void AddLimitedGamesFromLimitFile(List<string> limitedGames, FileInfo gameLimitFile,
            int checkedPlayersCount)
        {
            string[] lines = File.ReadAllLines(gameLimitFile.FullName);
            bool playersCountOutsideLimits =
                checkedPlayersCount < Convert.ToInt32(lines[0]) ||
                checkedPlayersCount > Convert.ToInt32(lines[1]);

            if (playersCountOutsideLimits)
            {
                limitedGames.Add(Path.GetFileNameWithoutExtension(gameLimitFile.Name));
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
            string path = GetFilePath(gameName);
            FilesCreator.CreateFileIfNotExists(path);
            using (TextWriter textWriter = new StreamWriter(path))
            {
                textWriter.WriteLine(Convert.ToString(minValue));
                textWriter.WriteLine(Convert.ToString(maxValue));
            }
        }

        public void DeleteFile(string gameName)
        {
            string path = GetFullDirectoryFilePath(gameName);
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
    }
}
