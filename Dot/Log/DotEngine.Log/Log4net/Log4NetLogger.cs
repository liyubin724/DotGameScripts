using log4net.Config;
using System;
using System.IO;
using System.Text;
using UnityEngine;
using ILog4NetLog = log4net.ILog;

namespace DotEngine.Log
{
    public class Log4NetLogger : ILogger
    {
        private bool isInited = false;
        public static ILogger Initalize(string xmlConfig)
        {
            if (string.IsNullOrEmpty(xmlConfig))
            {
                return null;
            }

            Log4NetLogger logger = new Log4NetLogger();
            byte[] configBytes = Encoding.UTF8.GetBytes(xmlConfig);
            using (MemoryStream ms = new MemoryStream(configBytes))
            {
                XmlConfigurator.Configure(ms);
                logger.isInited = true;
            }

            Application.logMessageReceived += OnMessageReceived;

            return logger;
        }

        private static void OnMessageReceived(string condition, string stackTrace, LogType type)
        {
            if (type == LogType.Exception)
            {
                LogUtil.LogFatal("Exception", $"{condition}\r\n{stackTrace}");
            }
        }

        private Log4NetLogger()
        {
        }

        private ILog4NetLog GetLogger(string loggerName)
        {
            if (string.IsNullOrEmpty(loggerName))
            {
                throw new ArgumentNullException("Log4NetLogger::GetLogger->loggerName is null or empty.");
            }

            if (!isInited)
            {
                return null;
            }

            return log4net.LogManager.GetLogger(loggerName);
        }


        public void LogDebug(string tagName, string msg)
        {
            GetLogger(tagName)?.Debug(msg);
        }

        public void LogInfo(string tagName, string msg)
        {
            GetLogger(tagName)?.Info(msg);
        }

        public void LogWarning(string tagName, string msg)
        {
            GetLogger(tagName)?.Warn(msg);
        }

        public void LogError(string tagName, string msg)
        {
            GetLogger(tagName)?.Error(msg);
        }

        public void LogFatal(string tagName, string msg)
        {
            GetLogger(tagName)?.Fatal(msg);
        }

        public void Log(LogLevelType levelType, string tag, string message)
        {
            if(levelType == LogLevelType.Debug)
            {
                LogDebug(tag, message);
            }else if(levelType == LogLevelType.Info)
            {
                LogInfo(tag, message);
            }else if(levelType == LogLevelType.Warning)
            {
                LogWarning(tag, message);
            }else if(levelType == LogLevelType.Error)
            {
                LogError(tag, message);
            }else if(levelType == LogLevelType.Fatal)
            {
                LogFatal(tag, message);
            }
        }
    }
}
