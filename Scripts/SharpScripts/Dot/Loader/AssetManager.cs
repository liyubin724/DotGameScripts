﻿using Dot.Core.Util;
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
            AssetPathMode pathMode, 
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
            },pathMode,maxLoadingCount,assetRootDir);
        }

        public AssetLoaderHandle LoadAssetAsync(
            string pathOrAddress,
            OnAssetLoadComplete complete, 
            AssetLoaderPriority priority = AssetLoaderPriority.Default,  
            OnAssetLoadProgress progress = null,
            SystemObject userData = null)
        {
            if(isInit)
            {
                return assetLoader.LoadOrInstanceBatchAssetAsync(new string[] { pathOrAddress }, false, priority, complete, progress, null, null, userData);
            }else
            {
                Debug.LogError("AssetManager::LoadAssetAsync->init is failed");
                return null;
            }
        }

        public AssetLoaderHandle LoadBatchAssetAsync(
            string[] pathOrAddresses,
            OnAssetLoadComplete complete,
            OnBatchAssetLoadComplete batchComplete,
            AssetLoaderPriority priority = AssetLoaderPriority.Default,
            OnAssetLoadProgress progress = null,
            OnBatchAssetsLoadProgress batchProgress = null,
            SystemObject userData = null)
        {
            if (isInit)
            {
                return assetLoader.LoadOrInstanceBatchAssetAsync(pathOrAddresses, false, priority, complete, progress, batchComplete, batchProgress, userData);
            }
            else
            {
                Debug.LogError("AssetManager::LoadAssetAsync->init is failed");
                return null;
            }
        }

        public AssetLoaderHandle InstanceAssetAsync(
            string pathOrAddress,
            OnAssetLoadComplete complete,
            AssetLoaderPriority priority = AssetLoaderPriority.Default,
            OnAssetLoadProgress progress = null,
            SystemObject userData = null)
        {
            if (isInit)
            {
                return assetLoader.LoadOrInstanceBatchAssetAsync(new string[] { pathOrAddress }, true, priority, complete, progress, null, null, userData);
            }
            else
            {
                Debug.LogError("AssetManager::LoadAssetAsync->init is failed");
                return null;
            }
        }

        public AssetLoaderHandle InstanceBatchAssetAsync(
            string[] pathOrAddresses,
            OnAssetLoadComplete complete,
            OnBatchAssetLoadComplete batchComplete,
            AssetLoaderPriority priority = AssetLoaderPriority.Default,
            OnAssetLoadProgress progress = null,
            OnBatchAssetsLoadProgress batchProgress = null,
            SystemObject userData = null)
        {
            if (isInit)
            {
                return assetLoader.LoadOrInstanceBatchAssetAsync(pathOrAddresses, true, priority, complete, progress, batchComplete, batchProgress, userData);
            }
            else
            {
                Debug.LogError("AssetManager::LoadAssetAsync->init is failed");
                return null;
            }
        }

        public UnityObject InstantiateAsset(string pathOrAddress,UnityObject asset)
        {
            if (isInit)
            {
                if(string.IsNullOrEmpty(pathOrAddress) || asset == null)
                {
                    Debug.LogError($"AssetManager::InstantiateAsset->asset is null or asset is null.assetPath = {(pathOrAddress ?? "")}");
                    return null;
                }
                return assetLoader?.InstantiateAsset(pathOrAddress, asset);
            }
            else
            {
                Debug.LogError("AssetManager::InstantiateAsset->init is failed");
                return null;
            }
        }

        public SceneLoaderHandle LoadSceneAsync(string pathOrAddress,
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
            return sceneLoader.LoadSceneAsync(pathOrAddress, completeCallback, progressCallback, loadMode, activateOnLoad, userData);
        }

        public void UnloadSceneAsync(string pathOrAddress,
            OnSceneUnloadComplete completeCallback,
            OnSceneUnloadProgress progressCallback,
            SystemObject userData = null)
        {
            if (sceneLoader == null)
            {
                Debug.LogError("AssetManager::LoadSceneAsync->sceneLoader has not been inited");
                return;
            }
            sceneLoader.UnloadSceneAsync(pathOrAddress, completeCallback, progressCallback, userData);
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
        
        public string[] GetAssetPathOrAddressByLabel(string label)
        {
            if(isInit && assetLoader!=null)
            {
                return assetLoader.GetAssetPathOrAddressByLabel(label);
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
