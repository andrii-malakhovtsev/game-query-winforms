using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace WhatGameToPlay
{
    public class PlayersDirectory : Directory
    {
        public PlayersDirectory(string name) : base(name) { }

        public override void CreateDirectoryIfNotExists()
        {
            if (FilesCreator.CreateDirectoryIfNotExists(_name))
                new PlayerFile("example player", directory: this);
        }

        public List<Player> PlayersList
        {
            get
            {
                var players = new List<Player>();
                foreach (FileInfo playerTextFile in TextFiles)
                {
                    AddPlayerFromTextFile(players, playerTextFile);
                }
                return players;
            }
        }

        public bool PlayerFileExists(string checkPlayer)
            => FilesReader.TextFileExist(TextFiles, checkPlayer);

        private static void AddPlayerFromTextFile(List<Player> players, FileInfo playerTextFile)
        {
            var gamesNotPlaying = File.ReadAllLines(playerTextFile.FullName).ToHashSet();
            players.Add(new Player(Path.GetFileNameWithoutExtension(playerTextFile.Name), gamesNotPlaying));
        }

        public string[] GetGamesPlayerDoesNotPlay(string selectedPlayer)
        {
            foreach (FileInfo file in TextFiles)
            {
                if (selectedPlayer == Path.GetFileNameWithoutExtension(file.Name))
                    return File.ReadAllLines(file.FullName);
            }
            return null;
        }

        public void WriteGamesNotPlayingToFile(string selectedPlayer,
            List<string> gamesNotPlayingList)
        {
            string path = GetFullDirectoryFilePath(selectedPlayer);
            new PlayerFile(selectedPlayer, directory: this); // change it later to getting player mb
            using (TextWriter textWriter = new StreamWriter(path))
            {
                foreach (string gameNotPlaying in gamesNotPlayingList)
                {
                    textWriter.WriteLine(gameNotPlaying.ToString());
                }
            }
        }

        public void DeleteGameFromPlayersFiles(string gameToDelete)
        {
            foreach (FileInfo fileInfo in TextFiles)
            {
                File.WriteAllLines(fileInfo.FullName,
                    File.ReadLines(fileInfo.FullName).Where(game => game != gameToDelete).ToList());
            }
        }
    }
}
