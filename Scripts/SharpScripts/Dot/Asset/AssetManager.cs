using Dot.Core.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemObject = System.Object;
using UnityObject = UnityEngine.Object;

namespace Dot.Asset
{
    public delegate void OnAssetLoadComplete(string address, UnityObject uObj, SystemObject userData);
    public delegate void OnAssetLoadProgress(string address, float progress, SystemObject userData);

    public delegate void OnBatchAssetLoadComplete(string[] addresses, UnityObject[] uObjs, SystemObject userData);
    public delegate void OnBatchAssetsLoadProgress(string[] addresses, float[] progresses, SystemObject userData);

    public enum AssetLoaderMode
    {
        AssetDatabase,
        AssetBundle,
    }

    public enum AssetLoaderPriority
    {
        VeryLow = 100,
        Low = 200,
        Default = 300,
        High = 400,
        VeryHigh = 500,
    }

    public partial class AssetManager : Singleton<AssetManager>
    {

        public void InitManager(AssetLoaderMode loaderMode,
            Action<bool> initCallback,
            int maxLoadingCount,
            string assetRootDir)
        {

        }

        private void LoadAssetAsync(string address,
            OnAssetLoadComplete complete,
            OnAssetLoadProgress progress,
            AssetLoaderPriority priority,
            SystemObject userData)
        {

        }

        private void LoadBatchAssetAsync(string[] addresses,
            OnAssetLoadComplete complete,
            OnBatchAssetLoadComplete batchComplete,
            OnAssetLoadProgress progress,
            OnBatchAssetsLoadProgress batchProgress,
            AssetLoaderPriority priority,
            SystemObject userData)
        {

        }

        private void InstantiateAssetAsync(string address,
            OnAssetLoadComplete complete,
            OnAssetLoadProgress progress,
            AssetLoaderPriority priority,
            SystemObject userData)
        {

        }

        private void InstantiateBatchAssetAsync(string[] addresses,
            OnAssetLoadComplete complete,
            OnBatchAssetLoadComplete batchComplete,
            OnAssetLoadProgress progress,
            OnBatchAssetsLoadProgress batchProgress,
            AssetLoaderPriority priority,
            SystemObject userData)
        {

        }

        public UnityObject InstantiateAsset(string address, UnityObject asset)
        {
            return null;
        }

        public void UnloadUnusedAsset(Action callback = null)
        {

        }

        public void CancelAssetAsync()
        {

        }

        public void DoUpdate(float deltaTime)
        {

        }

    }
}
