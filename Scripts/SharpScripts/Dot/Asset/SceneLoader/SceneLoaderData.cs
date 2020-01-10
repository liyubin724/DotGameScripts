using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;
using SystemObject = System.Object;

namespace Dot.Asset
{
    public enum SceneLoaderDataType
    {
        None = 0,
        Load,
        Unload,
    }

    public class SceneLoaderData
    {
        public string Address { get; set; }
        public SceneLoaderStageType StageType { get; set; }

        public void SetLoadData(string address,
            LoadSceneMode mode, 
            OnLoadSceneComplete loadComplete,
            OnLoadSceneProgress loadProgress,
            bool activateOnLoad = true,
            SystemObject userData = null)
        {

        }

        public void SetUnloadData(string address,
            OnUnloadSceneComplete unloadComplete,
            OnUnloadSceneProgress unloadProgress,
            SystemObject userData = null)
        {

        }
    }
}
