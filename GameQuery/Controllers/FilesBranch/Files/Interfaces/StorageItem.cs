namespace WhatGameToPlay
{
    public abstract class StorageItem
    {
        protected const string TextFileExtension = ".txt";

        protected StorageItem(string name) => _name = name;

        protected string _name;
    }
}
