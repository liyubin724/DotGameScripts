using DotEngine.Service.Log;
using log4net.Config;
using System;
using System.IO;
using System.Text;
using ILog4NetLog = log4net.ILog;

namespace DotEngine.Log
{
    public class Logger : ILogServicer
    {
        public static Logger CreateLogger(string xmlConfig,LogLevelType limitLevel = LogLevelType.Debug)
        {
            Logger logger = new Logger(limitLevel);
            if(logger.Initalize(xmlConfig))
            {
                return logger;
            }
            return null;
        }

        private LogLevelType limitLevelType = LogLevelType.Debug;
        private bool isInited = false;

        private Logger(LogLevelType levelType)
        {
            limitLevelType = levelType;
        }

        public void DoStart()
        {
        }

        public void DoDispose()
        {
        }

        private bool Initalize(string config)
        {
            if (string.IsNullOrEmpty(config))
            {
                return false;
            }

            byte[] configBytes = Encoding.UTF8.GetBytes(config);
            using (MemoryStream ms = new MemoryStream(configBytes))
            {
                try
                {
                    XmlConfigurator.Configure(ms);
                    isInited = true;
                    return true;
                }catch(Exception e)
                {
                    return false;
                }
            }
        }

        private ILog4NetLog GetLogger(string tag)
        {
            if(!string.IsNullOrEmpty(tag) && isInited)
            {
                return log4net.LogManager.GetLogger(tag);
            }
            return null;
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
            }
            else if(levelType == LogLevelType.Fatal)
            {
                LogFatal(tag, message);
            }
        }

        public void LogDebug(string tag, string message)
        {
            if(limitLevelType>=LogLevelType.Debug)
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
    }
}
