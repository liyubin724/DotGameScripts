using System.Runtime.InteropServices;

namespace DotEngine.Config.Ndb
{
    [StructLayout(LayoutKind.Sequential,Pack = 1)]
    public struct NDBHeader
    {
        public int fieldCount;
        public int lineCount;

        public int lineLength;

        public int dataOffset;
        public int arrayOffset;
        public int textOffset;

        public int version;
    }
}
