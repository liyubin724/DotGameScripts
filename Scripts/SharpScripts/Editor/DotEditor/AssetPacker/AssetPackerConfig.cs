using DotEditor.AssetFilter.AssetAddress;
using System.Collections.Generic;

namespace DotEditor.AssetPacker
{
    public class AssetPackerConfig 
    {
        public List<AssetPackerGroupData> groupDatas = new List<AssetPackerGroupData>();
    }

    public class AssetPackerGroupData
    {
        public string groupName = string.Empty;

        public bool isMain = false;
        public bool isPreload = false;
        public bool isNeverDestroy = false;

        public List<AssetPackerAddressData> assetFiles = new List<AssetPackerAddressData>();
    }

    public class AssetPackerAddressData
    {
        public string assetAddress;
        public string assetPath;
        public string bundlePath;
        public string[] labels = new string[0];
        public AssetCompressionMode compressionType = AssetCompressionMode.LZ4;
    }
}
