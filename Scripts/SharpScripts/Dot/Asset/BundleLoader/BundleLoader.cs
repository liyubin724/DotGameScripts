using Dot.Asset.Datas;
using Dot.Log;
using Dot.Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dot.Asset
{
    public class BundleLoader : AAssetLoader
    {
        private ObjectPool<BundleNode> bundleNodePool = new ObjectPool<BundleNode>();
        private Dictionary<string, BundleNode> bundleNodeDic = new Dictionary<string, BundleNode>();

        protected override void DoInitUpdate()
        {
            addressConfig = AssetConst.GetAddressConfig();
            if (addressConfig == null)
            {
                LogUtil.LogError(AssetConst.LOGGER_NAME, "Address config is null");
                State = AssetLoaderState.Error;
            }
            State = AssetLoaderState.Running;
        }

        protected override void OnDataUpdate(AssetLoaderData data)
        {
            
        }

        protected override void OnOperationFinished(AAsyncOperation operation)
        {
            
        }

        protected override void OnUnloadUnusedAsset()
        {
            
        }

        protected override void StartLoadingData(AssetLoaderData data)
        {
            
        }

        private bool GetBundleFilePath(string assetPath,out string filePath,out ulong offset)
        {
            offset = 0u;
            filePath = assetPath;

            return false;
        }
    }
}
