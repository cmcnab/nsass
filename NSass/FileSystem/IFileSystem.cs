﻿namespace NSass.FileSystem
{
    using System.IO;

    internal interface IFileSystem
    {
        Stream OpenFile(string filePath, FileMode mode);

        Stream OpenFile(string filePath, FileMode mode, FileAccess access);

        Stream OpenFile(string filePath, FileMode mode, FileAccess access, FileShare share);
    }
}
