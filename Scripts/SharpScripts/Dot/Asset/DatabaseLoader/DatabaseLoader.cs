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
            //addressConfig = AssetDatabase.LoadAssetAtPath<AssetAddressConfig>(AssetAddressConfig.CONFIG_PATH);
            //if(addressConfig!=null)
            //{
            //    State = AssetLoaderState.Running;
            //}else
            //{
            //    LogUtil.LogError(AAssetLoader.LOGGER_NAME, "AssetAddressConfig is null");
            //    State = AssetLoaderState.Error;
            //}
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

        protected override bool StartLoadingData(AssetLoaderData data)
        {
            foreach(var assetPath in data.paths)
            {
                operationList.Add(new DatabaseAsyncOperation(assetPath));
            }

            return true;
        }
    }

}
#endif