using System;
using System.IO;
using System.Text;

namespace DotEngine.FileSystem
{
    public class FileFragment
    {
        public static readonly int PATH_MAX_LENGTH = 128;
        public static readonly int LENGTH = sizeof(int) + PATH_MAX_LENGTH + sizeof(long) * 3;

        public string Path { get; set; } = null;
        public long Start { get; set; }
        public long Length { get; set; }
        public long Size { get; set; }
    }
}
