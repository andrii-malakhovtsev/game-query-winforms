using System.Collections.Generic;
using System.Windows.Forms;

namespace WhatGameToPlay
{
    public class Player
    {
        public Player(string name, HashSet<string> unplayedGames) : this(name)
        { 
            UnplayedGames = unplayedGames;
        }

        public Player(string name) => Name = name;

        public string Name { get; }

        public HashSet<string> UnplayedGames { get; }

        public CheckBox CheckBox { get; set; }
    }
}
