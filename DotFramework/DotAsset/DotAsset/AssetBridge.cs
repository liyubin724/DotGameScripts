using Dot.Core.Dispose;
using Dot.Core.Generic;
using Dot.Core.Pool;
using System.Collections.Generic;
using System.Linq;
using SystemObject = System.Object;
using UnityObject = UnityEngine.Object;

namespace Dot.Asset
{
    public class AssetBridge : IDispose
    {
        private UniqueIntID idCreator = new UniqueIntID(0);
        private AssetLoaderPriority loaderPriority = AssetLoaderPriority.Default;

        private static ObjectPool<AssetBridgeData> bridgeDataPool = new ObjectPool<AssetBridgeData>();

        private Dictionary<int, AssetBridgeData> bridgeDataDic = new Dictionary<int, AssetBridgeData>();

        public AssetBridge() { }
        public AssetBridge(AssetLoaderPriority priority)
        {
            loaderPriority = priority;
        }

        public int LoadAsset(string address, OnAssetLoadComplete complete, SystemObject userData = null)
        {
            AssetBridgeData bridgeData = bridgeDataPool.Get();
            bridgeData.uniqueID = idCreator.GetNextID();
            bridgeData.address = address;
            bridgeData.complete = complete;
            bridgeData.userData = userData;

            AssetHandler handler = AssetUtil.LoadAssetAsync(address, null, OnAssetComplete, loaderPriority, bridgeData);
            bridgeData.handler = handler;

            bridgeDataDic.Add(bridgeData.uniqueID, bridgeData);
            return bridgeData.uniqueID;
        }

        public int InstanceAsset(string address, OnAssetLoadComplete complete, SystemObject userData = null)
        {
            AssetBridgeData bridgeData = bridgeDataPool.Get();
            bridgeData.uniqueID = idCreator.GetNextID();
            bridgeData.address = address;
            bridgeData.complete = complete;
            bridgeData.userData = userData;

            AssetHandler handler = AssetUtil.InstanceAssetAsync(address, null, OnAssetComplete, loaderPriority, bridgeData);
            bridgeData.handler = handler;

            bridgeDataDic.Add(bridgeData.uniqueID, bridgeData);
            return bridgeData.uniqueID;
        }

        public int LoadAsset(string[] addresses, OnBatchAssetLoadComplete complete, SystemObject userData = null)
        {
            AssetBridgeData bridgeData = bridgeDataPool.Get();
            bridgeData.uniqueID = idCreator.GetNextID();
            bridgeData.addresses = addresses;
            bridgeData.batchComplete = complete;
            bridgeData.userData = userData;

            AssetHandler handler = AssetUtil.LoadBatchAssetAsync(addresses, null, null,null,OnBatchAssetComplete,loaderPriority, bridgeData);
            bridgeData.handler = handler;

            bridgeDataDic.Add(bridgeData.uniqueID, bridgeData);
            return bridgeData.uniqueID;
        }

        public int InstanceAsset(string[] addresses, OnBatchAssetLoadComplete complete, SystemObject userData = null)
        {
            AssetBridgeData bridgeData = bridgeDataPool.Get();
            bridgeData.uniqueID = idCreator.GetNextID();
            bridgeData.addresses = addresses;
            bridgeData.batchComplete = complete;
            bridgeData.userData = userData;

            AssetHandler handler = AssetUtil.InstanceBatchAssetAsync(addresses, null, null, null, OnBatchAssetComplete, loaderPriority, bridgeData);
            bridgeData.handler = handler;

            bridgeDataDic.Add(bridgeData.uniqueID, bridgeData);
            return bridgeData.uniqueID;
        }

        public void OnAssetComplete(string address, UnityObject uObj, SystemObject userData)
        {
            AssetBridgeData bridgeData = userData as AssetBridgeData;
            if(bridgeDataDic.ContainsKey(bridgeData.uniqueID))
            {
                bridgeDataDic.Remove(bridgeData.uniqueID);
                bridgeData.complete?.Invoke(address, uObj, bridgeData.userData);
            }
            bridgeDataPool.Release(bridgeData);
        }

        public void OnBatchAssetComplete(string[] addresses, UnityObject[] uObjs, SystemObject userData)
        {
            AssetBridgeData bridgeData = userData as AssetBridgeData;
            if (bridgeDataDic.ContainsKey(bridgeData.uniqueID))
            {
                bridgeDataDic.Remove(bridgeData.uniqueID);
                bridgeData.batchComplete?.Invoke(addresses, uObjs, bridgeData.userData);
            }
            
            bridgeDataPool.Release(bridgeData);
        }

        public void CancelLoad(int uniqueID)
        {
            if(bridgeDataDic.TryGetValue(uniqueID,out AssetBridgeData bridgeData))
            {
                bridgeDataDic.Remove(uniqueID);
                AssetUtil.UnloadAssetAsync(bridgeData.handler, true);
                bridgeDataPool.Release(bridgeData);
            }
        }

        public void Dispose()
        {
            int[] ids = bridgeDataDic.Keys.ToArray();
            foreach(var id in ids)
            {
                CancelLoad(id);
            }

            bridgeDataDic.Clear();
            bridgeDataPool.Clear();
        }

         class AssetBridgeData : IObjectPoolItem
         {
            public int uniqueID = -1;
            public bool isInstance = false;
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
                address = null;
                addresses = null;
                handler = null;
                complete = null;
                batchComplete = null;
                userData = null;
            }
        }
    }


}
