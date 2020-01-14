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

namespace Dot
{
    public class DotProxy : MonoBehaviour
    {
        private static string LOG_CONFIG = "LogConfig/log4net.xml";
        private static string LOG_CONFIG_IN_EDITOR = "LogConfig/log4net-editor.xml";

        public static void Startup(Action<bool> startupCallback)
        {
            DotProxy proxy = DontDestroyHandler.CreateComponent<DotProxy>();
            proxy.proxyStartupCallback = startupCallback;
        }

        public static void TearDown()
        {
            if (proxy != null)
            {
                UnityObject.Destroy(proxy.gameObject);
            }
        }

        public static DotProxy proxy = null;

        private Action<bool> proxyStartupCallback = null;
        private bool isStartup = false;

        private void Awake()
        {
            if(proxy!=null)
            {
                Destroy(this);
            }else
            {
                proxy = this;
                InitProxy();
            }
        }

        private void Update()
        {
            if(!isStartup)
            {
                return;
            }

            float deltaTime = Time.deltaTime;
            TimerManager.GetInstance().DoUpdate(deltaTime);
            LuaManager.GetInstance().DoUpdate(deltaTime);
            AssetManager.GetInstance().DoUpdate(deltaTime);
        }

        private void OnDestroy()
        {
            if (isStartup)
            {
                LuaManager.GetInstance().DoDispose();
                TimerManager.GetInstance().DoDispose();
                AssetManager.GetInstance().DoDispose();
            }
            proxy = null;
        }

        private void InitProxy()
        {
            StartCoroutine(InitLog((result) =>
            {
                isStartup = true;
                proxyStartupCallback.Invoke(true);
                proxyStartupCallback = null;
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

            if(webRequest.isDone)
            {
                byte[] datas = webRequest.downloadHandler.data;
                if(datas!=null && datas.Length>0)
                {
                    string configContent = Encoding.UTF8.GetString(datas);
                    LogManager.GetInstance().InitLog(configContent);

                    finishCallback.Invoke(true);
                }
                else
                {
                    Debug.Log("Init log failed");
                    finishCallback.Invoke(false);
                }
            }else
            {
                Debug.Log("Load log config failed");
                finishCallback.Invoke(false);
            }
        }
    }
}
