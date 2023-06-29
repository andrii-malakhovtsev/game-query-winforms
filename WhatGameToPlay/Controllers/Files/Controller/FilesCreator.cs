using System;
using System.IO;

namespace WhatGameToPlay
{
    public static class FilesCreator
    {
        public static void CreateStartingFiles()
        {
            if (CreateFile(FilesNames.ThemeFileName))
                FilesWriter.WriteThemeToFile(Theme.Standart.Name);

            if (CreateFile(FilesNames.OptionsFileName))
            {
                string trueOption = true.ToString(),
                      falseOption = false.ToString();

                string[] standartOptions = { trueOption, trueOption, trueOption, falseOption, trueOption, falseOption };
                FilesWriter.WriteOptionsToFile(standartOptions);
            }

            if (CreateFile(FilesNames.GamesListFileName))
                FilesWriter.AddGameToGameListFile("example game");

            if (CreateDirectory(FilesNames.PlayersDirectoryName))
                CreatePlayerFile("example player");

            CreateDirectory(FilesNames.LimitsDirectoryName);
        }

        public static void CreatePlayerFile(string selectedPlayer)
            => CreateFile(fileName: FilesReader.GetSelectedPlayerFilePath(selectedPlayer));

        public static bool CreateFile(string fileName)
        {
            return CreateFileType(fileName,
                existsCheckFunction: path => !File.Exists(fileName),
                createFunction: path => File.Create(fileName).Dispose());
        }

        private static bool CreateDirectory(string directoryName)
        {
            return CreateFileType(directoryName,
                existsCheckFunction: path => !Directory.Exists(path),
                createFunction: path => Directory.CreateDirectory(path));
        }

        private static bool CreateFileType(string path,
            Func<string, bool> existsCheckFunction,
            Action<string> createFunction)
        {
            bool doesNotExist = existsCheckFunction(path);
            if (doesNotExist)
            {
                createFunction(path);
            }
            return doesNotExist;
        }
    }
}
