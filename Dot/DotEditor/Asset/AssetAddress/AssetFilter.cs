using DotEditor.Utilities;
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
            return DirectoryUtility.GetAsset(assetFolder, isIncludeSubfolder, fileRegex);
        }
    }
}
