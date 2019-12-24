using System;

namespace DotEditor.AssetFilter.AssetAddress
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

    public enum AssetBundleNameType
    {
        Origin = 0,
        MD5,
    }

    [Serializable]
    public class AssetAddressOperation
    {
        public AssertPackMode packModeType = AssertPackMode.Together;
        public AssertAddressMode addressMode = AssertAddressMode.FullPath;
        public AssetBundleNameType bundleNameType = AssetBundleNameType.Origin;
        public string labels = string.Empty;
        public AssertCompressionMode compressionType = AssertCompressionMode.LZ4;
    }
}
