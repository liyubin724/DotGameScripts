using UnityEngine.SceneManagement;
using SystemObject = System.Object;

namespace Dot.Asset
{
    public delegate void OnSceneComplete(string address, Scene scene, SystemObject userData);
    public delegate void OnSceneProgress(string address, float progress, SystemObject userData);

    public partial class AssetManager
    {
        public SceneHandler LoadSceneAsync(string address,
            OnSceneComplete complete,
            OnSceneProgress progress,
            LoadSceneMode mode = LoadSceneMode.Single,
            bool activateOnLoad = true,
            SystemObject userData = null)
        {
            if(sceneLoader!=null)
            {
                return sceneLoader.LoadSceneAsync(address, complete, progress, mode, activateOnLoad, userData);
            }

            return null;
        }

        public SceneHandler UnloadSceneAsync(string address,
            OnSceneComplete complete,
            OnSceneProgress progress,
            SystemObject userData = null)
        {
            if(sceneLoader!=null)
            {
                return sceneLoader.UnloadSceneAsync(address, complete, progress, userData);
            }
            return null;
        }
    }
}
