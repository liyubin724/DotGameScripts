using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace Dot.Asset
{
    public abstract class ASceneLoader
    {
        protected AAssetLoader assetLoader = null;
        protected Dictionary<string, Scene> curSceneDic = new Dictionary<string, Scene>();

        protected List<SceneLoaderData> loaderDataList = new List<SceneLoaderData>();
        protected ASceneLoader(AAssetLoader loader)
        {
            assetLoader = loader;
        }

        internal void LoadScene()
        {

        }

        internal void UnloadScene()
        {

        }
    }
}
