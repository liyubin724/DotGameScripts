using Dot.Core.Util;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using SystemObject = System.Object;
using UnityObject = UnityEngine.Object;

namespace Dot.Core.Loader
{
    public class AssetManager : Singleton<AssetManager>
    {
        private AAssetLoader assetLoader = null;
        private SceneAssetLoader sceneLoader = null;
        private bool isInit = false;
        public void InitLoader(AssetLoaderMode loaderMode, 
            int maxLoadingCount, 
            string assetRootDir, 
            Action<bool> initCallback)
        {
            if (loaderMode == AssetLoaderMode.AssetBundle)
            {
                assetLoader = new AssetBundleLoader();
            }
            else if (loaderMode == AssetLoaderMode.AssetDatabase)
            {
#if UNITY_EDITOR
                assetLoader = new AssetDatabaseLoader();
#else
                Debug.LogError("AssetManager::InitLoader->AssetLoaderMode(AssetDatabase) can be used in Editor");
#endif
            }
            assetLoader?.Initialize((isSuccess) =>
            {
                isInit = isSuccess;
                if(isSuccess)
                {
                    sceneLoader = new SceneAssetLoader(loaderMode, assetLoader);
                }

                initCallback?.Invoke(isSuccess);
            },maxLoadingCount,assetRootDir);
        }

        public AssetLoaderHandle LoadAssetAsync(
            string address,
            OnAssetLoadComplete complete, 
            AssetLoaderPriority priority = AssetLoaderPriority.Default,  
            OnAssetLoadProgress progress = null,
            SystemObject userData = null)
        {
            if(isInit)
            {
                return assetLoader.LoadOrInstanceBatchAssetAsync(new string[] { address }, false, priority, complete, progress, null, null, userData);
            }else
            {
                Debug.LogError("AssetManager::LoadAssetAsync->init is failed");
                return null;
            }
        }

        public AssetLoaderHandle LoadBatchAssetAsync(
            string[] address,
            OnAssetLoadComplete complete,
            OnBatchAssetLoadComplete batchComplete,
            AssetLoaderPriority priority = AssetLoaderPriority.Default,
            OnAssetLoadProgress progress = null,
            OnBatchAssetsLoadProgress batchProgress = null,
            SystemObject userData = null)
        {
            if (isInit)
            {
                return assetLoader.LoadOrInstanceBatchAssetAsync(address, false, priority, complete, progress, batchComplete, batchProgress, userData);
            }
            else
            {
                Debug.LogError("AssetManager::LoadAssetAsync->init is failed");
                return null;
            }
        }

        public AssetLoaderHandle InstanceAssetAsync(
            string address,
            OnAssetLoadComplete complete,
            AssetLoaderPriority priority = AssetLoaderPriority.Default,
            OnAssetLoadProgress progress = null,
            SystemObject userData = null)
        {
            if (isInit)
            {
                return assetLoader.LoadOrInstanceBatchAssetAsync(new string[] { address }, true, priority, complete, progress, null, null, userData);
            }
            else
            {
                Debug.LogError("AssetManager::LoadAssetAsync->init is failed");
                return null;
            }
        }

        public AssetLoaderHandle InstanceBatchAssetAsync(
            string[] address,
            OnAssetLoadComplete complete,
            OnBatchAssetLoadComplete batchComplete,
            AssetLoaderPriority priority = AssetLoaderPriority.Default,
            OnAssetLoadProgress progress = null,
            OnBatchAssetsLoadProgress batchProgress = null,
            SystemObject userData = null)
        {
            if (isInit)
            {
                return assetLoader.LoadOrInstanceBatchAssetAsync(address, true, priority, complete, progress, batchComplete, batchProgress, userData);
            }
            else
            {
                Debug.LogError("AssetManager::LoadAssetAsync->init is failed");
                return null;
            }
        }

        public UnityObject InstantiateAsset(string address,UnityObject asset)
        {
            if (isInit)
            {
                if(string.IsNullOrEmpty(address) || asset == null)
                {
                    Debug.LogError($"AssetManager::InstantiateAsset->asset is null or asset is null.assetPath = {(address ?? "")}");
                    return null;
                }
                return assetLoader?.InstantiateAsset(address, asset);
            }
            else
            {
                Debug.LogError("AssetManager::InstantiateAsset->init is failed");
                return null;
            }
        }

        public SceneLoaderHandle LoadSceneAsync(string address,
            OnSceneLoadComplete completeCallback,
            OnSceneLoadProgress progressCallback,
            LoadSceneMode loadMode = LoadSceneMode.Single,
            bool activateOnLoad = true,
            SystemObject userData = null)
        {
            if(sceneLoader == null)
            {
                Debug.LogError("AssetManager::LoadSceneAsync->sceneLoader has not been inited");
                return null;
            }
            return sceneLoader.LoadSceneAsync(address, completeCallback, progressCallback, loadMode, activateOnLoad, userData);
        }

        public void UnloadSceneAsync(string address,
            OnSceneUnloadComplete completeCallback,
            OnSceneUnloadProgress progressCallback,
            SystemObject userData = null)
        {
            if (sceneLoader == null)
            {
                Debug.LogError("AssetManager::LoadSceneAsync->sceneLoader has not been inited");
                return;
            }
            sceneLoader.UnloadSceneAsync(address, completeCallback, progressCallback, userData);
        }

        public void UnloadUnusedAsset(Action callback = null)
        {
            if (isInit)
            {
                assetLoader?.UnloadUnusedAssets(callback);
            }
            else
            {
                Debug.LogError("AssetManager::InstantiateAsset->init is failed");
            }
        }

        public void UnloadAssetLoader(AssetLoaderHandle handle, bool destroyIfLoaded = false)
        {
            if (isInit)
            {
               assetLoader?.UnloadAssetLoader(handle, destroyIfLoaded);
            }
            else
            {
                Debug.LogError("AssetManager::InstantiateAsset->init is failed");
            }
        }
        
        public string[] GetAssetAddressByLabel(string label)
        {
            if(isInit && assetLoader!=null)
            {
                return assetLoader.GetAssetAddressByLabel(label);
            }
            return null;
        }

        public void DoUpdate(float deltaTime)
        {
            assetLoader?.DoUpdate(deltaTime);
            sceneLoader?.DoUpdate(deltaTime);
        }
    }
}
