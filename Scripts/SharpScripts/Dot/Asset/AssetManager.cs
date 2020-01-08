using Dot.Asset.Datas;
using Dot.Core.Util;
using Dot.Log;
using System;
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
        private AAssetLoader assetLoader = null;
        public void InitManager(AssetLoaderMode loaderMode,
            Action<bool> initCallback,
            string assetRootDir = "")
        {
            if(loaderMode == AssetLoaderMode.AssetBundle)
            {
                assetLoader = new BundleLoader();
            }
#if UNITY_EDITOR
            else
            {
                assetLoader = new DatabaseLoader();
            }
#endif

            if(assetLoader == null)
            {
                LogUtil.LogError(AssetConst.LOGGER_NAME, "AssetLoader is Null");
                initCallback?.Invoke(false);
            }else
            {
                assetLoader.Initialize((result) =>
                {
                    if(!result)
                    {
                        LogUtil.LogError(AssetConst.LOGGER_NAME, "AssetManager::InitManager->init failed");
                    }

                    initCallback?.Invoke(result);
                },  assetRootDir);
            }
        }

        public void ChangeMaxLoadingCount(int count)
        {
            if(assetLoader!=null)
            {
                assetLoader.MaxLoadingCount = count;
            }else
            {
                LogUtil.LogError(AssetConst.LOGGER_NAME, "AssetManager::ChangeMaxLoadingCount->assetloader is null");
            }
        }

        public void ChangeAutoCleanInterval(float interval)
        {
            if (assetLoader != null)
            {
                assetLoader.AutoCleanInterval = interval;
            }
            else
            {
                LogUtil.LogError(AssetConst.LOGGER_NAME, "AssetManager::ChangeAutoCleanInterval->assetloader is null");
            }
        }

        public AssetHandler LoadAssetAsync(string label,
            OnAssetLoadComplete complete,
            OnBatchAssetLoadComplete batchComplete,
            OnAssetLoadProgress progress = null,
            OnBatchAssetsLoadProgress batchProgress = null,
            AssetLoaderPriority priority = AssetLoaderPriority.Default,
            SystemObject userData = null)
        {
            return LoadBatchAssetAsync(label,null,false, complete, batchComplete, progress, batchProgress, priority, userData);
        }

        public AssetHandler LoadAssetAsync(string assetPath,
            OnAssetLoadComplete complete,
            OnAssetLoadProgress progress = null,
            AssetLoaderPriority priority = AssetLoaderPriority.Default,
            SystemObject userData = null)
        {
            return LoadBatchAssetAsync(null, new string[] { assetPath }, false, complete, null, progress, null, priority, userData);
        }

        public AssetHandler LoadAssetAsync(string[] addresses, 
            OnAssetLoadComplete complete,
            OnBatchAssetLoadComplete batchComplete,
            OnAssetLoadProgress progress = null,
            OnBatchAssetsLoadProgress batchProgress = null,
            AssetLoaderPriority priority = AssetLoaderPriority.Default,
            SystemObject userData = null)
        {
            return LoadBatchAssetAsync(null, addresses,false, complete, batchComplete, progress, batchProgress, priority, userData);
        }

        public AssetHandler InstanceAssetAsync(string label,
            OnAssetLoadComplete complete,
            OnBatchAssetLoadComplete batchComplete,
            OnAssetLoadProgress progress = null,
            OnBatchAssetsLoadProgress batchProgress = null,
            AssetLoaderPriority priority = AssetLoaderPriority.Default,
            SystemObject userData = null)
        {
            return LoadBatchAssetAsync(label, null, true, complete, batchComplete, progress, batchProgress, priority, userData);
        }

        public AssetHandler InstanceAssetAsync(string assetPath,
            OnAssetLoadComplete complete,
            OnAssetLoadProgress progress = null,
            AssetLoaderPriority priority = AssetLoaderPriority.Default,
            SystemObject userData = null)
        {
            return LoadBatchAssetAsync(null, new string[] { assetPath }, true, complete, null, progress, null, priority, userData);
        }

        public AssetHandler InstanceAssetAsync(string[] addresses,
            OnAssetLoadComplete complete,
            OnBatchAssetLoadComplete batchComplete,
            OnAssetLoadProgress progress = null,
            OnBatchAssetsLoadProgress batchProgress = null,
            AssetLoaderPriority priority = AssetLoaderPriority.Default,
            SystemObject userData = null)
        {
            return LoadBatchAssetAsync(null, addresses, true, complete, batchComplete, progress, batchProgress, priority, userData);
        }

        private AssetHandler LoadBatchAssetAsync(
            string label, 
            string[] addresses,
            bool isInstance,
            OnAssetLoadComplete complete,
            OnBatchAssetLoadComplete batchComplete,
            OnAssetLoadProgress progress,
            OnBatchAssetsLoadProgress batchProgress,
            AssetLoaderPriority priority,
            SystemObject userData)
        {
            if(assetLoader == null)
            {
                LogUtil.LogError(AssetConst.LOGGER_NAME, "AssetManager::LoadBatchAssetAsync->assetLoader is Null");
                return null;
            }

            return assetLoader.LoadBatchAssetAsync(label, addresses, isInstance, complete, batchComplete, progress, batchProgress, priority, userData);
        }

        public void UnloadAssetAsync(AssetHandler handler,bool destroyIfIsInstnace = false)
        {
            if (assetLoader == null)
            {
                LogUtil.LogError(AssetConst.LOGGER_NAME, "AssetManager::UnloadAssetAsync->assetLoader is Null");
                return;
            }

            assetLoader.UnloadAssetAsync(handler, destroyIfIsInstnace);
        }

        public UnityObject InstantiateAsset(string address, UnityObject asset)
        {
            if (assetLoader == null)
            {
                LogUtil.LogError(AssetConst.LOGGER_NAME, "AssetManager::InstantiateAsset->assetLoader is Null");
                return null;
            }

            return assetLoader.InstantiateAsset(address, asset);
        }

        public void UnloadUnusedAsset(Action callback = null)
        {
            if (assetLoader == null)
            {
                LogUtil.LogError(AssetConst.LOGGER_NAME, "AssetManager::UnloadUnusedAsset->assetLoader is Null");
                return;
            }

            assetLoader.UnloadUnusedAsset(callback);
        }

        public void DoUpdate(float deltaTime)
        {
            assetLoader?.DoUpdate(deltaTime);
        }

        public override void DoDispose()
        {
            assetLoader?.DoDispose();
            base.DoDispose();
        }

    }
}
