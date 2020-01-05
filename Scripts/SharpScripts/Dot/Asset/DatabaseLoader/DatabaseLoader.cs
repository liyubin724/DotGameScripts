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
            for (int i = 0; i < data.Paths.Length; ++i)
            {
                string assetPath = data.Paths[i];
                if (!assetNodeDic.TryGetValue(assetPath, out AAssetNode assetNode))
                {
                    assetNode = CreateAssetNode(assetPath);
                }
                assetNode.Retain();
            }

            for (int i =0;i<data.paths.Length;++i)
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
                data.AddNode(i, assetNode);
            }
        }

        private DatabaseAssetNode CreateAssetNode(string assetPath)
        {
            DatabaseAssetNode assetNode = new DatabaseAssetNode();
            assetNode.InitNode(assetPath);

            DatabaseAsyncOperation operation = new DatabaseAsyncOperationxcf gff gfffv();
        }
    }

}
#endif