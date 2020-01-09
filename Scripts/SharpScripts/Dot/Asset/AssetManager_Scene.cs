using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;
using SystemObject = System.Object;

namespace Dot.Asset
{

    public delegate void OnSceneLoadComplete(string address, Scene scene, SystemObject userData);
    public delegate void OnSceneLoadProgress(string address, float progress, SystemObject userData);

    public delegate void OnSceneUnloadComplete(string address, SystemObject userData);
    public delegate void OnSceneUnloadProgress(string address, float progress, SystemObject userData);

    public enum SceneLoaderStageType
    {
        None = 0,
        Unload,
        Loading,
        Instancing,
        Finished,
    }

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
