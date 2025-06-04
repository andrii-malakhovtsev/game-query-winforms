using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace WhatGameToPlay
{
    public class PlayersDirectory : Directory
    {
        public PlayersDirectory(string name) : base(name) { }

        public override void CreateDirectoryIfMissing()
        {
            if (FilesCreator.CreateDirectoryIfMissing(_name))
            {
                new PlayerFile("example player", directory: this);
            }
        }

        public HashSet<Player> PlayersList
        {
            get
            {
                var players = new HashSet<Player>();
                foreach (FileInfo playerTextFile in TextFiles)
                {
                    AddPlayerFromTextFile(players, playerTextFile);
                }
                return players;
            }
        }

        private static void AddPlayerFromTextFile(HashSet<Player> players, FileInfo playerTextFile)
        {
            var unplayedGames = File.ReadAllLines(playerTextFile.FullName).ToHashSet();
            players.Add(new Player(Path.GetFileNameWithoutExtension(playerTextFile.Name), unplayedGames));
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

        public void WriteGamesNotPlayingToFile(string selectedPlayer, List<string> gamesNotPlayingList)
        {
            new PlayerFile(selectedPlayer, directory: this); // change it later to getting player mb

            using (TextWriter textWriter = new StreamWriter(GetFilePath(selectedPlayer)))
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
