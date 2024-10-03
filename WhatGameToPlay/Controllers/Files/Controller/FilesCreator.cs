using System;
using System.Collections.Generic;
using System.IO;

namespace WhatGameToPlay
{
    public static class FilesCreator
    {
        public static void CreateStartingFiles(HashSet<StorageItem> storageItems)
        {
            foreach (var storageItem in storageItems)
            {
                if (storageItem is IFileCreator file)
                    file.CreateFileIfMissing();

                if (storageItem is Directory directory)
                    directory.CreateDirectoryIfMissing();
            }
        }

        public static bool CreateFileIfMissing(string fileName)
        {
            return CreateFileTypeIfMissing(fileName,
                fileTypeMissing: path => !File.Exists(fileName),
                createFileType: path => File.Create(fileName).Dispose());
        }

        public static bool CreateDirectoryIfMissing(string directoryName)
        {
            return CreateFileTypeIfMissing(directoryName,
                fileTypeMissing: path => !System.IO.Directory.Exists(path),
                createFileType: path => System.IO.Directory.CreateDirectory(path));
        }

        private static bool CreateFileTypeIfMissing(string path,
            Func<string, bool> fileTypeMissing,
            Action<string> createFileType)
        {
            bool missing = fileTypeMissing(path);

            if (missing)
            {
                createFileType(path);
            }
            return missing;
        }
    }
}
