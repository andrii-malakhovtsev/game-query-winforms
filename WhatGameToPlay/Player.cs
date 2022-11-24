using System.Collections.Generic;
using System.Windows.Forms;

namespace WhatGameToPlay
{
    public class Player
    {
        public string Name { get; set; }
        public List<string> GamesNotPlaying { get; set; }
        public CheckBox CheckBox { get; set; }

        public Player(string name, List<string> gamesNotPlaying)
        {
            Name = name;
            GamesNotPlaying = gamesNotPlaying;
        }
    }
}
