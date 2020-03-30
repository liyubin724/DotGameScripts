using Dot.Core.Generic;
using Dot.Core.Dispose;
using Dot.Core.Pool;
using System.Collections.Generic;
using SystemObject = System.Object;
using UnityObject = UnityEngine.Object;

namespace Dot.Asset
{
    public class AssetBridgeData : IObjectPoolItem
    {
        public long uniqueID = -1;
        public string address = null;
        public string[] addresses = null;
        public AssetHandler handler = null;
        public OnAssetLoadComplete complete = null;
        public OnBatchAssetLoadComplete batchComplete = null;
        public SystemObject userData = null;

        public void OnGet()
        {
        }

        public void OnNew()
        {
        }

        public void OnRelease()
        {
            uniqueID = -1;
            handler = null;
            complete = null;
            batchComplete = null;
            userData = null;
        }
    }

    public class AssetBridge : ABaseDispose
    {
        private AssetLoaderPriority loaderPriority = AssetLoaderPriority.Default;
        private UniqueIntID idCreator = new UniqueIntID();

        private static ObjectPool<AssetBridgeData> bridgeDataPool = new ObjectPool<AssetBridgeData>();

        private List<AssetBridgeData> bridgeDatas = new List<AssetBridgeData>();

        public AssetBridge() { }
        public AssetBridge(AssetLoaderPriority priority)
        {
            loaderPriority = priority;
        }

        public long LoadAsset(string address,OnAssetLoadComplete complete,SystemObject userData = null)
        {
            AssetBridgeData bridgeData = bridgeDataPool.Get();
            bridgeData.address = address;
            bridgeData.uniqueID = idCreator.Next();
            bridgeData.complete = complete;
            bridgeData.userData = userData;

            AssetHandler handler = AssetManager.GetInstance().LoadAssetAsync(address, OnAssetComplete, null, loaderPriority, bridgeData);
            bridgeData.handler = handler;

            bridgeDatas.Add(bridgeData);

            return bridgeData.uniqueID;
        }

        public long InstanceAsset(string address,OnAssetLoadComplete complete, SystemObject userData = null)
        {
            AssetBridgeData bridgeData = bridgeDataPool.Get();
            bridgeData.address = address;
            bridgeData.uniqueID = idCreator.Next();
            bridgeData.complete = complete;
            bridgeData.userData = userData;

            AssetHandler handler = AssetManager.GetInstance().InstanceAssetAsync(address, OnAssetComplete, null, loaderPriority, bridgeData);
            bridgeData.handler = handler;

            bridgeDatas.Add(bridgeData);

            return bridgeData.uniqueID;
        }

        public long LoadAsset(string[] addresses, OnBatchAssetLoadComplete complete,SystemObject userData = null)
        {
            AssetBridgeData bridgeData = bridgeDataPool.Get();
            bridgeData.addresses = addresses;
            bridgeData.uniqueID = idCreator.Next();
            bridgeData.batchComplete = complete;
            bridgeData.userData = userData;

            AssetHandler handler = AssetManager.GetInstance().LoadAssetAsync(addresses, null, OnBatchAssetComplete, null, null, loaderPriority, bridgeData);
            bridgeData.handler = handler;

            bridgeDatas.Add(bridgeData);

            return bridgeData.uniqueID;
        }

        public void OnAssetComplete(string address,UnityObject uObj,SystemObject userData)
        {
            AssetBridgeData bridgeData = userData as AssetBridgeData;
            bridgeDatas.Remove(bridgeData);

            bridgeData.complete?.Invoke(address, uObj, bridgeData.userData);

            bridgeDataPool.Release(bridgeData);
        }

        public void OnBatchAssetComplete(string[] addresses,UnityObject[] uObjs,SystemObject userData)
        {
            AssetBridgeData bridgeData = userData as AssetBridgeData;
            bridgeDatas.Remove(bridgeData);

            bridgeData.batchComplete?.Invoke(addresses, uObjs, bridgeData.userData);

            bridgeDataPool.Release(bridgeData);
        }

        public void CancelLoadAsset(long id)
        {
            for(int i =0;i<bridgeDatas.Count -1;++i)
            {
                var bridgeData = bridgeDatas[i];
                if(bridgeData.uniqueID == id)
                {
                    AssetManager.GetInstance().UnloadAssetAsync(bridgeData.handler, true);
                    bridgeDatas.RemoveAt(i);

                    bridgeDataPool.Release(bridgeData);
                    return;
                }
            }
        }

        public AssetBridgeData GetBridgeData(long id)
        {
            foreach(var data in bridgeDatas)
            {
                if(data.uniqueID == id)
                {
                    return data;
                }
            }
            return null;
        }

        protected override void DisposeManagedResource()
        {
            for(int i = bridgeDatas.Count -1;i>=0;--i)
            {
                var data = bridgeDatas[i];
                AssetManager.GetInstance().UnloadAssetAsync(data.handler);
                bridgeDataPool.Release(data);
            }
            bridgeDatas.Clear();
        }

        protected override void DisposeUnmanagedResource()
        {
            
        }
    }
}
