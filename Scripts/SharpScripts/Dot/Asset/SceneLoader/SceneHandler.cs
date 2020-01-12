using UnityEngine;
using UnityEngine.SceneManagement;

namespace Dot.Asset
{
    public class SceneHandler
    {
        public string Address { get;private set; }
        public string SceneName { get; private set; }
        public string ScenePath { get; private set; }

        public float Progress { get; internal set; }
        public Scene TargetScene { get; internal set; }

        public void ActiveScene()
        {
            if (TargetScene.IsValid() && TargetScene.isLoaded)
            {
                GameObject[] gObjs = TargetScene.GetRootGameObjects();
                foreach(var gObj in gObjs)
                {
                    gObj.SetActive(true);
                }
            }
        }
    }
}
