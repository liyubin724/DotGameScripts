using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using SystemObject = System.Object;

namespace Dot.Asset
{
    public enum SceneLoaderDataState
    {
        None = 0,
        
        Load,
        Unload,

        Loading,
        Unloading,
        
        Instancing,
        Finished,
    }

    public class SceneLoaderData
    {
        internal string address;
        internal string scenePath;
        internal string sceneName;
        internal OnSceneComplete completeCallback = null;
        internal OnSceneProgress progressCallback = null;
        internal SystemObject userData =null;
        internal LoadSceneMode sceneMode = LoadSceneMode.Single;
        internal bool isActiveWhenLoaded = true;

        internal SceneHandler handler;
        internal SceneLoaderDataState state = SceneLoaderDataState.None;

        public void InitLoadData(string address,string path,
            OnSceneComplete complete,OnSceneProgress progress,
            LoadSceneMode sceneMode,bool isActive,
            SystemObject userData)
        {
            this.address = address;
            this.scenePath = path;
            this.sceneName = Path.GetFileNameWithoutExtension(path);
            completeCallback = complete;
            progressCallback = progress;
            this.sceneMode = sceneMode;
            this.isActiveWhenLoaded = isActive;
            this.userData = userData;

            handler = new SceneHandler();
            state = SceneLoaderDataState.Load;
        }

        public void InitUnloadData(string address, string path,
            OnSceneComplete complete, OnSceneProgress progress,
            SystemObject userData)
        {
            this.address = address;
            this.scenePath = path;
            this.sceneName = Path.GetFileNameWithoutExtension(path);
            completeCallback = complete;
            progressCallback = progress;
            this.userData = userData;

            handler = new SceneHandler();
            state = SceneLoaderDataState.Unload;
        }

        internal void DoComplete(Scene scene)
        {
            state = SceneLoaderDataState.Finished;
            handler.TargetScene = scene;

            if(!isActiveWhenLoaded)
            {
                GameObject[] objs = scene.GetRootGameObjects();
                foreach(var obj in objs)
                {
                    obj.SetActive(false);
                }
            }

            progressCallback?.Invoke(address, 1.0f, userData);
            completeCallback?.Invoke(address, scene, userData);
        }

        internal void DoProgress(float progress)
        {
            handler.Progress = progress;
            progressCallback?.Invoke(address, progress, userData);
        }
    }
}
