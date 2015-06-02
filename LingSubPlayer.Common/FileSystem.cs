using System.IO;

namespace LingSubPlayer.Common
{
    public class FileSystem : IFileSystem
    {
        public bool FileExists(string fileName)
        {
            return File.Exists(fileName);
        }
    }
}