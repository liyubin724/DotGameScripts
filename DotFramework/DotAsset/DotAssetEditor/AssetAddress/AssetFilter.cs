using DotEditor.Core.Util;
using System;

namespace DotEditor.Asset.AssetAddress
{
    [Serializable]
    public class AssetFilter
    {
        public string assetFolder = "Assets";
        public bool isIncludeSubfolder = true;

        public string fileRegex = string.Empty;

        public string[] Filter()
        {
            return DirectoryUtil.GetAsset(assetFolder, isIncludeSubfolder, fileRegex);
        }
    }
}
