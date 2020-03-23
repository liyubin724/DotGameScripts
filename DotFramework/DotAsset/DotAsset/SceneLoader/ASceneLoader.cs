using Dot.Asset.Datas;
using Dot.Log;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using SystemObject = System.Object;

namespace Dot.Asset
{
    public abstract class ASceneLoader
    {
        protected AAssetLoader assetLoader = null;
        protected List<SceneLoaderData> loaderDatas = new List<SceneLoaderData>();
        protected SceneLoaderData currentLoaderData = null;
        protected AsyncOperation asyncOperation = null;

        protected ASceneLoader(AAssetLoader loader)
        {
            assetLoader = loader;
        }

        public SceneHandler LoadSceneAsync(string address,
            OnSceneComplete complete,
            OnSceneProgress progress,
            LoadSceneMode mode,
            bool activateOnLoad,
            SystemObject userData)
        {
            if(assetLoader != null)
            {
                string scenePath = assetLoader.GetAssetPathByAddress(address);
                if(string.IsNullOrEmpty(scenePath))
                {
                    LogUtil.LogError(AssetConst.LOGGER_NAME, "scenePath is null.address = " + address);
                    return null;
                }
                SceneLoaderData data = new SceneLoaderData();
                data.InitLoadData(address, scenePath, complete, progress, mode, activateOnLoad, userData);
                loaderDatas.Add(data);

                return data.handler;
            }else
            {
                LogUtil.LogError(AssetConst.LOGGER_NAME, "assetloader is not init");
                return null;
            }
        }

        public SceneHandler UnloadSceneAsync(string address,
            OnSceneComplete complete,
            OnSceneProgress progress,
            SystemObject userData)
        {
            if (assetLoader != null)
            {
                string scenePath = assetLoader.GetAssetPathByAddress(address);
                if (string.IsNullOrEmpty(scenePath))
                {
                    LogUtil.LogError(AssetConst.LOGGER_NAME, "scenePath is null.address = " + address);
                    return null;
                }
                SceneLoaderData data = new SceneLoaderData();
                data.InitUnloadData(address, scenePath, complete, progress, userData);
                loaderDatas.Add(data);

                return data.handler;
            }
            else
            {
                LogUtil.LogError(AssetConst.LOGGER_NAME, "assetloader is not init");
                return null;
            }
        }

        internal void DoUpdate(float deltaTime)
        {
            if(assetLoader == null)
            {
                return;
            }
            if(currentLoaderData != null)
            {
                UpdateLoaderData();
            }else if(loaderDatas.Count>0)
            {
                SceneLoaderData data = loaderDatas[0];
                loaderDatas.RemoveAt(0);

#if UNITY_EDITOR || UNITY_STANDALONE
                string sceneName = data.sceneName;
                if(data.state == SceneLoaderDataState.Load)
                {
                    Scene scene = SceneManager.GetSceneByName(sceneName);
                    if(scene.isLoaded)
                    {
                        LogUtil.LogError(AssetConst.LOGGER_NAME, "scene has been loaded");
                        return;
                    }
                }else if(data.state == SceneLoaderDataState.Unload)
                {
                    Scene scene = SceneManager.GetSceneByName(sceneName);
                    if (!scene.isLoaded)
                    {
                        LogUtil.LogError(AssetConst.LOGGER_NAME, "the scene has not been loaded");
                        return;
                    }
                }
#endif
                currentLoaderData = data;
            }
        }

        protected abstract void UpdateLoaderData();
    }
}
