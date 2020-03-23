using Dot.Asset.Datas;
using Dot.Log;
using Dot.Core;
using System;
using SystemObject = System.Object;
using UnityObject = UnityEngine.Object;
using Dot.Core.Proxy;

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
        private ASceneLoader sceneLoader = null;

        private AssetLoaderMode loaderMode = AssetLoaderMode.AssetDatabase;

        public void InitManager(AssetLoaderMode mode,
            Action<bool> initCallback,
            string assetRootDir = "")
        {
            loaderMode = mode;

            LogUtil.LogInfo(AssetConst.LOGGER_DEBUG_NAME, $"AssetManager::InitManager->Start init mgr.mode = {mode.ToString()},assetRootDir = {assetRootDir}");

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

                    LogUtil.LogInfo(AssetConst.LOGGER_DEBUG_NAME, "AssetManager::InitManager->init Success");

                    if (loaderMode == AssetLoaderMode.AssetBundle)
                    {
                        sceneLoader = new BundleSceneLoader(assetLoader);
                    }
#if UNITY_EDITOR
                    else
                    {
                        sceneLoader = new DatabaseSceneLoader(assetLoader);
                    }
#endif
                    StartAutoClean();

                    initCallback?.Invoke(result);
                },  assetRootDir);
            }
        }

        public void ChangeMaxLoadingCount(int count)
        {
            if(assetLoader!=null)
            {
                LogUtil.LogInfo(AssetConst.LOGGER_DEBUG_NAME, $"AssetManager::ChangeMaxLoadingCount->Change Count from {assetLoader.MaxLoadingCount} to {count}");
                assetLoader.MaxLoadingCount = count;
            }else
            {
                LogUtil.LogError(AssetConst.LOGGER_NAME, "AssetManager::ChangeMaxLoadingCount->assetloader is null");
            }
        }

        public AssetHandler LoadAssetAsyncByLabel(string label,
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

        protected override void DoInit()
        {
            UpdateProxy.GetInstance().DoUpdateHandle += DoUpdate;
        }

        private void DoUpdate(float deltaTime)
        {
            assetLoader?.DoUpdate(deltaTime);
            sceneLoader?.DoUpdate(deltaTime);
        }

        public override void DoDispose()
        {
            UpdateProxy.GetInstance().DoUpdateHandle -= DoUpdate;
            DoDispose_Clean();

            assetLoader?.DoDispose();
            base.DoDispose();
        }

    }
}
