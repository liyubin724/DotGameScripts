using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;
using SystemObject = System.Object;

namespace Dot.Asset
{
    public enum SceneLoaderStageType
    {
        None = 0,
        Unload,
        Loading,
        Instancing,
        Finished,
    }

    public delegate void OnLoadSceneComplete(string address, Scene scene, SystemObject userData);
    public delegate void OnLoadSceneProgress(string address, SceneLoaderStageType stageType,float progress, SystemObject userData);

    public delegate void OnUnloadSceneComplete(string address, SystemObject userData);
    public delegate void OnUnloadSceneProgress(string address, float progress, SystemObject userData);

    public partial class AssetManager
    {
        public void LoadSceneAsync(string address,LoadSceneMode mode ,bool activateOnLoad,SystemObject userData)
        {

        }

        public void UnloadSceneAsync(string address,SystemObject userData)
        {

        }
    }
}
