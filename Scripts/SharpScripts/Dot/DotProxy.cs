using Dot.Core.Loader;
using Dot.Core.Timer;
using Dot.Core.Util;
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

        private static DotProxy proxy = null;

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
            TimerManager.GetInstance().DoUpdate(Time.deltaTime);
            LuaManager.GetInstance().DoUpdate(Time.deltaTime);
        }

        private void OnDestroy()
        {
            LuaManager.GetInstance().DoDispose();
            TimerManager.GetInstance().DoDispose();

            proxy = null;
        }

    }
}
