using System;
using System.Collections.Generic;
using System.IO;

namespace WhatGameToPlay
{
    public class GamesLimitsDirectory : Directory
    {
        public GamesLimitsDirectory(string name) : base(name) { }

        public override void CreateDirectoryIfMissing() => FilesCreator.CreateDirectoryIfMissing(_name);

        private string GetFilePath(string gameName) => base.GetFilePath(gameName);

        public HashSet<string> GetLimitedGamesList(int checkedPlayersCount)
        {
            var limitedGames = new HashSet<string>();
            foreach (FileInfo gamesLimitsTextFileInfo in TextFiles)
            {
                AddLimitedGamesFromLimitFile(limitedGames, gamesLimitsTextFileInfo, checkedPlayersCount);
            }
            return limitedGames;
        }

        private static void AddLimitedGamesFromLimitFile(HashSet<string> limitedGames, FileInfo gameLimitFile,
            int checkedPlayersCount)
        {
            string[] limits = File.ReadAllLines(gameLimitFile.FullName);
            
            bool playersCountOutsideLimits =
                checkedPlayersCount < Convert.ToInt32(limits[0]) ||
                checkedPlayersCount > Convert.ToInt32(limits[1]);

            if (playersCountOutsideLimits)
            {
                limitedGames.Add(Path.GetFileNameWithoutExtension(gameLimitFile.Name));
            }
        }

        public bool GetPlayersLimits(string gameName, out decimal[] limits)
        {
            const int LimitsCount = 2;
            limits = new decimal[LimitsCount];

            foreach (FileInfo file in TextFiles)
            {
                if (gameName == Path.GetFileNameWithoutExtension(file.Name))
                {
                    SetLimitsFromFile(limits, file);
                    return true;
                }
            }
            return false;
        }

        private static void SetLimitsFromFile(decimal[] limits, FileInfo file)
        {
            string[] fileRead = File.ReadAllLines(file.FullName);
            for (int i = 0; i < limits.Length; i++)
            {
                limits[i] = Convert.ToDecimal(fileRead[i]);
            }
        }

        public void WriteLimitsToFile(string gameName, decimal minValue, decimal maxValue)
        {
            string path = GetFilePath(gameName);
            FilesCreator.CreateFileIfMissing(path);

            using (TextWriter textWriter = new StreamWriter(path))
            {
                textWriter.WriteLine(Convert.ToString(minValue));
                textWriter.WriteLine(Convert.ToString(maxValue));
            }
        }

        public void DeleteFile(string gameName) // to another class
        {
            string path = base.GetFilePath(gameName);
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
    }
}
