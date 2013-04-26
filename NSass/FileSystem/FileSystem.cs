namespace NSass.FileSystem
{
    using System.Diagnostics.CodeAnalysis;
    using System.IO;

    [ExcludeFromCodeCoverage]
    internal class FileSystem : IFileSystem
    {
        public Stream OpenFile(string filePath, FileMode mode)
        {
            return File.Open(filePath, mode);
        }

        public Stream OpenFile(string filePath, FileMode mode, FileAccess access)
        {
            return File.Open(filePath, mode, access);
        }

        public Stream OpenFile(string filePath, FileMode mode, FileAccess access, FileShare share)
        {
            return File.Open(filePath, mode, access, share);
        }
    }
}
