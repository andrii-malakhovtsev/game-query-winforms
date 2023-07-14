using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace WhatGameToPlay
{
    public class GamesListFile: TextFile, IFileCreator, IFileWriter
    {
        public GamesListFile(string name) : base(name) {}

        public void CreateFileIfNotExists()
        {
            if (FilesCreator.CreateFileIfNotExists(_name))
                WriteToFile("example game");
        }

        public string[] CurrentGames => File.ReadAllLines(_name);

        public List<string> CurrentGamesList => File.ReadAllLines(_name).OrderBy(game => game).ToList();

        public void WriteToFile(params string[] gameName)
        {
            foreach (string game in gameName)
                File.AppendAllText(_name, game + Environment.NewLine);
        }

        public void DeleteFromFile(string gameToDelete)
        {
            string[] gamesExceptGameToDelete = CurrentGames.Where(game => game != gameToDelete).ToArray();

            File.WriteAllText(_name, string.Empty);

            foreach (string game in gamesExceptGameToDelete)
                WriteToFile(game);
        }
    }
}
