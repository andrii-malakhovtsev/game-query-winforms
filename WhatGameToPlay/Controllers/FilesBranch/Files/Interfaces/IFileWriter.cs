namespace WhatGameToPlay
{
    public interface IFileWriter
    {
        void WriteToFile(params string[] toWrite);
    }
}
