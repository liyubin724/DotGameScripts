using Dot.Manager;
using UnityEngine;

namespace Dot.Log
{
    public class LogManager : BaseSingletonManager<LogManager>
    {
        private const string OUTPUT_DIR_TAG = "#OUTPUT_DIR#";

        private bool isInit = false;
        public void InitLog(string logConfig)
        {
            if (!string.IsNullOrEmpty(logConfig))
            {
                logConfig = logConfig.Replace(OUTPUT_DIR_TAG, GetOutputDir());
                LogUtil.Initalize(logConfig);

                isInit = true;
#if !UNITY_EDITOR
                Application.logMessageReceived += OnException;
#endif
            }
        }

        private void OnException(string condition, string stackTrace, LogType type)
        {
            if(type == LogType.Exception)
            {
                LogUtil.LogError("Exception", stackTrace);
            }
        }

        public override void DoDispose()
        {
            if (isInit)
            {
#if !UNITY_EDITOR
                Application.logMessageReceived -= OnException;
#endif
                isInit = false;
            }
            base.DoDispose();
        }

        private string GetOutputDir()
        {
#if UNITY_EDITOR
            string dir = Application.dataPath;
            dir = dir.Substring(0, dir.Length - "/Assets".Length);
            return dir;
#else
            return Application.persistentDataPath; 
#endif
        }
    }
}
