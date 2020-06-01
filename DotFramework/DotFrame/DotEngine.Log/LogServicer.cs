using DotEngine.Framework.Services;
using log4net;
using log4net.Config;
using System.IO;
using System.Text;

#if !UNITY_EDITOR
using UnityEngine;
#endif

namespace DotEngine.Log
{
    public class LogServicer : ILogService
    {
        public static LogServicer GetLogServicer(string xmlConfig,LogLevelType limitLevelType)
        {
            if(string.IsNullOrEmpty(xmlConfig))
            {
                return null;
            }

            LogServicer logger = new LogServicer(limitLevelType);
            if(logger.Initalize(xmlConfig))
            {
                return logger;
            }
            return null;
        }

        private LogLevelType limitLevelType = LogLevelType.Debug;
        private bool isInited = false;

        private LogServicer(LogLevelType limitLevelType)
        {
            this.limitLevelType = limitLevelType;
        }

        private bool Initalize(string xmlConfig)
        {
            byte[] configBytes = Encoding.UTF8.GetBytes(xmlConfig);
            try
            {
                using (MemoryStream ms = new MemoryStream(configBytes))
                {
                    XmlConfigurator.Configure(ms);
                    isInited = true;
                    return true;
                }
            }
            catch
            {
                isInited = false;
                return false;
            }
        }

        private ILog GetLogger(string tag)
        {
            if(isInited)
            {
                try
                {
                    return LogManager.GetLogger(tag);
                }
                catch
                {
                }
            }
            return null;
        }

        public void DoRemove()
        {
#if !UNITY_EDITOR
            Application.logMessageReceived -= OnMessageReceived;   
#endif
        }

        public void DoRegister()
        {
#if !UNITY_EDITOR
            if(isInited)
            {
                Application.logMessageReceived += OnMessageReceived;
            }
#endif
        }

#if !UNITY_EDITOR
        private void OnMessageReceived(string condition,string stackTrace, LogType type)
        {
            if (type == LogType.Exception)
            {
                LogFatal("Exception", $"{condition}\r\n{stackTrace}");
            }
        }
#endif

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

        public void LogDebug(string tag, string message)
        {
            if(limitLevelType>= LogLevelType.Debug)
            {
                GetLogger(tag)?.Debug(message);
            }
        }

        public void LogInfo(string tag, string message)
        {
            if (limitLevelType >= LogLevelType.Info)
            {
                GetLogger(tag)?.Debug(message);
            }
        }

        public void LogWarning(string tag, string message)
        {
            if (limitLevelType >= LogLevelType.Warning)
            {
                GetLogger(tag)?.Debug(message);
            }
        }

        public void LogError(string tag, string message)
        {
            if (limitLevelType >= LogLevelType.Error)
            {
                GetLogger(tag)?.Debug(message);
            }
        }

        public void LogFatal(string tag, string message)
        {
            if (limitLevelType >= LogLevelType.Fatal)
            {
                GetLogger(tag)?.Debug(message);
            }
        }
    }
}
