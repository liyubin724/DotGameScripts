using UnityEngine;
using Dot.Utilities;

namespace Dot.Proxy
{
    public class StartupProxy : MonoBehaviour
    {
        public static void Startup()
        {
            DontDestroyHandler.CreateComponent<StartupProxy>();
        }

        private bool isStartup = false;
        private UpdateProxy updateProxy = null;
        void Awake()
        {
            if(isStartup)
            {
                return;
            }
            isStartup = true;
            updateProxy = UpdateProxy.GetInstance();
        }
        
        void Update()
        {
            updateProxy.DoUpdate(Time.deltaTime);
            updateProxy.DoUnscaleUpdate(Time.unscaledDeltaTime);
        }

        void LateUpdate()
        {
            updateProxy.DoLateUpdate();
        }

        void OnDestroy()
        {
            updateProxy.DoDispose();
            isStartup = false;
            updateProxy = null;
        }
    }
}
