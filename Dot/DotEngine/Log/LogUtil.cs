using System;
using UnityEngine;

namespace Dot.Core.Log
{
    public static class LogUtil
    {
        private static LogLevelType logLevel = LogLevelType.Info;
        private static ILog logger = null;

        public static void SetLogger(ILog logger,LogLevelType logLevel)
        {
            LogUtil.logLevel = logLevel;
            LogUtil.logger = logger;

            if(logger!=null)
            {
                Application.logMessageReceived += OnMessageReceived;
            }
        }

        private static void OnMessageReceived(string condition, string stackTrace, LogType type)
        {
            if (type == LogType.Exception)
            {
                LogFatal("Exception", $"{condition}\r\n{stackTrace}");
            }
        }

        #region Debug
        public static void LogDebug(string tagName, string msg)
        {
            if (logger != null && logLevel <= LogLevelType.Debug)
            {
                logger.LogDebug(tagName, msg);
            }
        }

        public static void LogDebug(Type tagType, string msg)
        {
            if (logger != null && logLevel <= LogLevelType.Debug)
            {
                logger.LogDebug(tagType, msg);
            }
        }
        #endregion

        #region Info
        public static void LogInfo(string tagName, string msg)
        {
            if (logger != null && logLevel <= LogLevelType.Info)
            {
                logger.LogInfo(tagName, msg);
            }
        }

        public static void LogInfo(Type tagType, string msg)
        {
            if (logger != null && logLevel <= LogLevelType.Info)
            {
                logger.LogInfo(tagType, msg);
            }
        }
        #endregion

        #region Warning
        public static void LogWarning(Type tagType, string msg)
        {
            if (logger != null && logLevel <= LogLevelType.Warning)
            {
                logger.LogWarning(tagType, msg);
            }
        }

        public static void LogWarning(string tagName, string msg)
        {
            if (logger != null && logLevel <= LogLevelType.Warning)
            {
                logger.LogWarning(tagName, msg);
            }
        }
        #endregion

        #region Error
        public static void LogError(Type tagType, string msg)
        {
            if (logger != null && logLevel <= LogLevelType.Error)
            {
                logger.LogError(tagType, msg);
            }
        }

        public static void LogError(string tagName, string msg)
        {
            if (logger != null && logLevel <= LogLevelType.Error)
            {
                logger.LogError(tagName, msg);
            }
        }
        #endregion

        #region Fatal
        private static void LogFatal(string tagName, string msg)
        {
            if (logger != null)
            {
                logger.LogFatal(tagName, msg);
            }
        }
        #endregion
    }
}
