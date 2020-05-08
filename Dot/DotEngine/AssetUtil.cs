using UnityEngine;

namespace Dot.Core.Asset
{
    public delegate GameObject InstanceAssetHandler(string assetName, GameObject asset);
    public static class AssetUtil
    {
        public static InstanceAssetHandler InstanceAsset { get; set; } = null;
    }
}
