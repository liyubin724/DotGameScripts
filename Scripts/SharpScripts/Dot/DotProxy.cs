using Dot.Asset;
using Dot.Timer;
using Dot.Util;
using Dot.Log;
using Dot.Lua;
using UnityEngine;
using UnityObject = UnityEngine.Object;
using System;
using UnityEngine.Networking;
using System.Collections;
using System.Text;
using Dot.Dispatch;

namespace Dot
{
    public class DotProxy : MonoBehaviour
    {
        private static string LOG_CONFIG = "LogConfig/log4net.xml";
        private static string LOG_CONFIG_IN_EDITOR = "LogConfig/log4net-editor.xml";

        public static void Startup()
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

        private bool isStartup = false;
        public bool IsStartup { get => isStartup; }

        private TimerManager timerMgr = null;
        private EventManager eventMgr = null;
        private AssetManager assetMgr = null;
        private LuaManager luaMgr = null;
        private LogManager logMgr = null;

        private void Awake()
        {
            if(proxy!=null)
            {
                Destroy(this);
                return;
            }

            proxy = this;

            timerMgr = TimerManager.GetInstance();
            eventMgr = EventManager.GetInstance();
            assetMgr = AssetManager.GetInstance();
            luaMgr = LuaManager.GetInstance();
            logMgr = LogManager.GetInstance();

            InitProxy();
        }

        private void Update()
        {
            if(!isStartup)
            {
                return;
            }

            float deltaTime = Time.deltaTime;
            timerMgr.DoUpdate(deltaTime);
            luaMgr.DoUpdate(deltaTime);
            assetMgr.DoUpdate(deltaTime);
        }

        private void OnDestroy()
        {
            if (isStartup)
            {
                luaMgr.DoDispose();
                assetMgr.DoDispose();
                eventMgr.DoDispose();
                timerMgr.DoDispose();
            }
            proxy = null;
        }

        private void InitProxy()
        {
            StartCoroutine(InitLog((result) =>
            {
                isStartup = true;
                eventMgr.TriggerEvent(EventConst.PROXY_LOG_INIT, result);

                eventMgr.TriggerEvent(EventConst.PROXY_INIT, isStartup);
            }));
        }

        private IEnumerator InitLog(Action<bool> finishCallback)
        {
            string logConfigPath;
#if UNITY_EDITOR
            logConfigPath = $"{Application.streamingAssetsPath}/{LOG_CONFIG_IN_EDITOR}";
#else

#if DEBUG
            logConfigPath = $"{Application.streamingAssetsPath}/{LOG_CONFIG_IN_EDITOR}";
#else
            logConfigPath = $"{Application.streamingAssetsPath}/{LOG_CONFIG}";
#endif

#endif
            UnityWebRequest webRequest = UnityWebRequest.Get(logConfigPath);
            yield return webRequest.SendWebRequest();

            bool initLogResult = false;
            if(webRequest.isDone)
            {
                byte[] datas = webRequest.downloadHandler.data;
                if(datas!=null && datas.Length>0)
                {
                    string configContent = Encoding.UTF8.GetString(datas);
                    logMgr.InitLog(configContent);

                    initLogResult = true;
                }
            }
            yield return new WaitForEndOfFrame();

            finishCallback.Invoke(initLogResult);
        }
    }
}
