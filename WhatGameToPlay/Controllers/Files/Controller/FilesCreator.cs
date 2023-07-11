using System;
using System.Collections.Generic;
using System.IO;

namespace WhatGameToPlay
{
    public static class FilesCreator
    {
        public static void CreateStartingFiles(HashSet<StorageItem> fileTypes)
        {
            foreach (var file in fileTypes)
            {
                if (file is IFileCreator fileCreator)
                    fileCreator.CreateFileIfNotExists();

                if (file is Directory directoryCreator)
                    directoryCreator.CreateDirectoryIfNotExists();
            }
        }

        public static bool CreateFileIfNotExists(string fileName)
        {
            return CreateFileTypeIfNotExists(fileName,
                existsCheckFunction: path => !File.Exists(fileName),
                createFunction: path => File.Create(fileName).Dispose());
        }

        public static bool CreateDirectoryIfNotExists(string directoryName)
        {
            return CreateFileTypeIfNotExists(directoryName,
                existsCheckFunction: path => !System.IO.Directory.Exists(path),
                createFunction: path => System.IO.Directory.CreateDirectory(path));
        }

        public static bool CreateFileTypeIfNotExists(string path,
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
