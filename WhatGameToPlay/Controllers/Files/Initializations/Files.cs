using System.Collections.Generic;

namespace WhatGameToPlay
{
    public class Files : FileTypeObjects
    {
        public Files()
        {
            Theme = new ThemeFile(name: "Theme");
            Options = new OptionsFile(name: "Options");
            GamesList = new GamesListFile(name: "GamesList");
            _filesObjects = new HashSet<StorageItem> { Theme, Options, GamesList };
        }

        public ThemeFile Theme { get; }

        public OptionsFile Options { get; }

        public GamesListFile GamesList { get; }
    }
}
