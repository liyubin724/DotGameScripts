using System;

namespace DotEditor.AssertFilter.AssetAddress
{
    public enum AssertCompressionMode
    {
        Uncompressed = 0,
        LZ4,
        LZMA,
    }

    public enum AssertPackMode
    {
        Together,
        Separate,
    }

    public enum AssertAddressMode
    {
        FullPath,
        FileName,
        FileNameWithoutExtension,
    }

    [Serializable]
    public class AssetAddressOperation
    {
        public AssertPackMode packModeType = AssertPackMode.Together;
        public AssertAddressMode addressMode = AssertAddressMode.FullPath;
        public bool isMD5ForBundleName = false;
        public string labels = string.Empty;
        public AssertCompressionMode compressionType = AssertCompressionMode.LZ4;
    }
}
