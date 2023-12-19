using System.Collections.Generic;

namespace WhatGameToPlay
{
    public abstract class FileTypeObjects
    {
        protected HashSet<StorageItem> _filesObjects;

        public static explicit operator HashSet<StorageItem>(FileTypeObjects fileTypeObjects)
        {
            return fileTypeObjects._filesObjects;
        }
    }
}
