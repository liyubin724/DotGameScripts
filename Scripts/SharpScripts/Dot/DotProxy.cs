using Dot.Core.Loader;
using Dot.Core.Timer;
using Dot.Core.Util;
using Dot.Log;
using Dot.Lua;
using UnityEngine;
using UnityObject = UnityEngine.Object;

namespace Dot
{
    public class DotProxy : MonoBehaviour
    {
        public static void StartUp()
        {
            DontDestroyHandler.CreateComponent<DotProxy>();
        }

        public static void TearDown()
        {
            if (proxy != null)
            {
                UnityObject.Destroy(proxy.gameObject);
            }
        }

        public static DotProxy proxy = null;

        private void Awake()
        {
            if(proxy!=null)
            {
                Destroy(this);
            }else
            {
                proxy = this;
            }
        }

        private void Update()
        {
            float deltaTime = Time.deltaTime;

            TimerManager.GetInstance().DoUpdate(deltaTime);
            LuaManager.GetInstance().DoUpdate(deltaTime);
            AssetManager.GetInstance().DoUpdate(deltaTime);
        }

        private void OnDestroy()
        {
            LuaManager.GetInstance().DoDispose();
            TimerManager.GetInstance().DoDispose();
            AssetManager.GetInstance().DoDispose();

            proxy = null;
        }

        public void InitLog(string logConfig) => LogManager.GetInstance().InitLog(logConfig);
    }
}
