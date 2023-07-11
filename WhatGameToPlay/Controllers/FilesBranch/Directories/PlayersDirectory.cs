using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace WhatGameToPlay
{
    public class PlayersDirectory : Directory // sort all classes by Create/Delete/Add methods etc.
    { // check mb constants can become private and check all methods to
        public PlayersDirectory(string name) : base(name) {}

        public override FileInfo[] TextFiles
            => GetTextFiles(directoryName: _name); // mb to interface somehow or maybe even abstract classes

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

        public string[] GetGamesPlayerDoesNotPlay(string selectedPlayer) // mb to PlayerFile later
        {
            foreach (FileInfo file in TextFiles)
            {
                if (selectedPlayer == Path.GetFileNameWithoutExtension(file.Name))
                    return File.ReadAllLines(file.FullName);
            }
            return null;
        }

        private static void AddPlayerFromTextFile(List<Player> players, FileInfo playerTextFile)
        {
            var gamesNotPlaying = File.ReadAllLines(playerTextFile.FullName).ToHashSet();
            players.Add(new Player(Path.GetFileNameWithoutExtension(playerTextFile.Name), gamesNotPlaying));
        }

        public void AppendGameToPlayersFiles(string gameName) // mb combine with "WriteToFiles"
        {
            foreach (FileInfo file in TextFiles)
            {
                File.AppendAllText(file.FullName, gameName + "\n");
            }
        }

        public void WriteGamesNotPlayingToFile(string selectedPlayer,
            List<string> gamesNotPlayingList)
        {
            string path = GetFullDirectoryFilePath(_name, selectedPlayer);
            new PlayerFile(selectedPlayer, directory: this); // change it later to getting player
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
