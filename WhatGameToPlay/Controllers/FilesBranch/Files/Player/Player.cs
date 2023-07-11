using System.Collections.Generic;
using System.Windows.Forms;

namespace WhatGameToPlay
{
    public class Player
    {
        public Player(string name, HashSet<string> gamesNotPlaying) : this(name)
            => GamesNotPlaying = gamesNotPlaying;

        public Player(string name) => Name = name;

        public string Name { get; }

        public HashSet<string> GamesNotPlaying { get; }

        public CheckBox CheckBox { get; set; }
    }
}
