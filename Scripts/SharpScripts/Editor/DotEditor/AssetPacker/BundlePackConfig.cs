using System;
using UnityEditor;

namespace DotEditor.AssetPacker
{
    public enum CompressOption
    {
        Uncompressed = 0,
        StandardCompression,
        ChunkBasedCompression,
    }

    public enum ValidBuildTarget
    {
        iOS = 9,
        Android = 13,
        StandaloneWindows64 = 19,
    }

    [Serializable]
    public class BundlePackConfig
    {
        public string bundleOutputDir = "D:/assetbundle";
        public bool cleanupBeforeBuild = false;
        public ValidBuildTarget buildTarget = ValidBuildTarget.StandaloneWindows64;
        public CompressOption compression = CompressOption.StandardCompression;

        public BuildAssetBundleOptions bundleOptions = BuildAssetBundleOptions.DeterministicAssetBundle;

        internal BuildTarget GetBuildTarget()
        {
            return (BuildTarget)buildTarget;
        }

        internal BuildAssetBundleOptions GetBundleOptions()
        {
            BuildAssetBundleOptions options = bundleOptions;
            if (compression == CompressOption.Uncompressed)
            {
                return options | BuildAssetBundleOptions.UncompressedAssetBundle;
            }
            else if (compression == CompressOption.ChunkBasedCompression)
            {
                return options | BuildAssetBundleOptions.ChunkBasedCompression;
            }
            else
            {
                return options;
            }
        }
    }
}
