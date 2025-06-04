using System.Collections.Generic;

namespace WhatGameToPlay
{
    public class Directories : FileTypeObjects
    {
        public Directories()
        {
            Players = new PlayersDirectory(name: "Players");
            GamesLimits = new GamesLimitsDirectory(name: "Limits");
            _filesObjects = new HashSet<StorageItem> { Players, GamesLimits };
        }

        public PlayersDirectory Players { get; }

        public GamesLimitsDirectory GamesLimits { get; }
    }
}
