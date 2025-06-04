namespace WhatGameToPlay
{
    public abstract class TextFile : StorageItem
    {
        protected TextFile(string name) : base(name)
        {
            _name += TextFileExtension;
        }
    }
}
