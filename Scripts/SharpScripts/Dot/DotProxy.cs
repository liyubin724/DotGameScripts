﻿using Dot.Asset;
using Dot.Dispatch;
using Dot.Log;
using Dot.Lua;
using Dot.Timer;
using Dot.Util;
using System;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityObject = UnityEngine.Object;

namespace Dot
{

    public class DotProxy : MonoBehaviour
    {
#if UNITY_EDITOR || DEBUG
        private static string LOG_CONFIG = "LogConfig/log4net-editor.xml";
#else
        private static string LOG_CONFIG = "LogConfig/log4net.xml";
#endif

        public static void Startup(Action<bool> initCallback)
        {
            DotProxy proxy = DontDestroyHandler.CreateComponent<DotProxy>();
            proxy.initFinishedCallback = initCallback;
        }

        public static void TearDown()
        {
            if (proxy != null)
            {
                UnityObject.Destroy(proxy.gameObject);
            }
        }

        public static DotProxy proxy = null;

        private UpdateProxy updateProxy = null;
        private bool isStartup = false;
        public bool IsStartup { get => isStartup; }


        private Action<bool> initFinishedCallback = null;

        private void Awake()
        {
            if(proxy!=null)
            {
                Destroy(this);
                return;
            }

            proxy = this;

            updateProxy = UpdateProxy.GetInstance();

            TimerManager.GetInstance();
            EventManager.GetInstance();
            AssetManager.GetInstance();
            LuaManager.GetInstance();
            LogManager.GetInstance();

            InitProxy();
        }

        private void Update()
        {
            if(!isStartup)
            {
                return;
            }

            updateProxy.DoUpdate(Time.deltaTime);
            updateProxy.DoUnscaleUpdate(Time.unscaledDeltaTime);
        }

        private void LateUpdate()
        {
            updateProxy.DoLateUpdate();
        }

        private void OnDestroy()
        {
            TimerManager.GetInstance().DoDispose();
            EventManager.GetInstance().DoDispose();
            AssetManager.GetInstance().DoDispose();
            LuaManager.GetInstance().DoDispose();
            LogManager.GetInstance().DoDispose();

            updateProxy.DoDispose();
            updateProxy = null;

            proxy = null;
        }

        private void InitProxy()
        {
            StartCoroutine(InitLog((result) =>
            {
                isStartup = true;

                initFinishedCallback?.Invoke(result);
            }));
        }

        private IEnumerator InitLog(Action<bool> finishCallback)
        {
            string logConfigPath = $"{Application.streamingAssetsPath}/{LOG_CONFIG}";

            UnityWebRequest webRequest = UnityWebRequest.Get(logConfigPath);
            yield return webRequest.SendWebRequest();

            bool initLogResult = false;
            if(webRequest.isDone)
            {
                byte[] datas = webRequest.downloadHandler.data;
                if(datas!=null && datas.Length>0)
                {
                    string configContent = Encoding.UTF8.GetString(datas);
                    LogManager.GetInstance().InitLog(configContent);

                    initLogResult = true;
                }
            }
            yield return new WaitForEndOfFrame();

            finishCallback.Invoke(initLogResult);
        }
    }
}
