using System.Collections.Generic;

namespace WhatGameToPlay
{
    public class Directories : FileTypeObjects
    {
        public Directories()
        {
            Players = new PlayersDirectory(name: "Players");
            PlayersLimits = new PlayersLimitsDirectory(name: "Limits");
            _filesObjects = new HashSet<StorageItem> { Players, PlayersLimits };
        }

        public PlayersDirectory Players { get; }

        public PlayersLimitsDirectory PlayersLimits { get; }
    }
}
