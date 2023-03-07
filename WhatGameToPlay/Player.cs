using System.Collections.Generic;
using System.Windows.Forms;

namespace WhatGameToPlay
{
    public class Player
    {
        public Player(string name, List<string> gamesNotPlaying)
        {
            Name = name;
            GamesNotPlaying = gamesNotPlaying;
        }

        public string Name { get; }

        public List<string> GamesNotPlaying { get; }

        public CheckBox CheckBox { get; set; }
    }
}
