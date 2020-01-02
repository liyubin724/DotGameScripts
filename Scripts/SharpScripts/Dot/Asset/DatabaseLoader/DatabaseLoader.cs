#if UNITY_EDITOR

using Dot.Asset.Datas;
using Dot.Log;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Dot.Asset
{
    public class DatabaseLoader : AAssetLoader
    {
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
            for (int i = 0; i < data.paths.Length; ++i)
            {
                string assetPath = data.paths[i];
                DatabaseAssetNode assetNode = assetNodeDic[assetPath] as DatabaseAssetNode;


            }
        }

        protected override void OnOperationFinished(AAsyncOperation operation)
        {
            string assetPath = operation.AssetPath;
            if(assetNodeDic.TryGetValue(assetPath,out AAssetNode assetNode))
            {
                DatabaseAssetNode node = assetNode as DatabaseAssetNode;
                node.SetAsset(operation.GetAsset());
            }
        }

        protected override void OnUnloadUnusedAsset()
        {
            
        }

        protected override void StartLoadingData(AssetLoaderData data)
        {
            for(int i =0;i<data.paths.Length;++i)
            {
                string assetPath = data.paths[i];
                if (!assetNodeDic.TryGetValue(assetPath, out AAssetNode assetNode))
                {
                    assetNode = new DatabaseAssetNode();
                    assetNode.InitNode(assetPath);

                    operations.AddOrUpdate(assetPath, new DatabaseAsyncOperation()
                    {
                        AssetPath = assetPath,
                    });
                }

                assetNode.Retain();
                data.AddAssetNode(i, assetNode);
            }
        }
    }

}
#endif