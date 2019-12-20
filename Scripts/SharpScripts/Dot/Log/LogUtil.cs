using log4net;
using log4net.Config;
using System;
using System.IO;
using System.Text;

namespace Dot.Log
{
    public static class LogUtil
    {
        private static bool isLogInit = false;

        public static void Initalize(string xmlConfig)
        {
            byte[] configBytes = Encoding.UTF8.GetBytes(xmlConfig);
            using (MemoryStream ms = new MemoryStream(configBytes))
            {
                XmlConfigurator.Configure(ms);
                isLogInit = true;
            }
        }

        public static ILog Logger(string loggerName)
        {
            if (isLogInit)
            {
                return log4net.LogManager.GetLogger(loggerName);
            }
            return null;
        }
        public static void LogError(Type type, string message)
        {
            LogError(type.Name, message);
        }

        public static void LogError(string loggerName, string message)
        {
            Logger(loggerName)?.Error(message);
        }

        public static void LogErrorFormat(string loggerName, string msgFormat, params object[] args)
        {
            Logger(loggerName)?.ErrorFormat(loggerName, msgFormat, args);
        }
        public static void LogWarning(Type type, string message)
        {
            LogWarning(type.Name, message);
        }

        public static void LogWarning(string loggerName, string message)
        {
            Logger(loggerName)?.Warn(message);
        }

        public static void LogWarningFormat(string loggerName, string msgFormat, params object[] args)
        {
            Logger(loggerName)?.WarnFormat(loggerName, msgFormat, args);
        }
        public static void LogInfo(Type type, string message)
        {
            LogInfo(type.Name, message);
        }

        public static void LogInfo(string loggerName, string message)
        {
            Logger(loggerName)?.Info(message);
        }

        public static void LogInfoFormat(string loggerName, string msgFormat, params object[] args)
        {
            Logger(loggerName)?.InfoFormat(msgFormat, args);
        }

        public static void LogFatal(Type type, string message)
        {
            LogFatal(type.Name, message);
        }

        public static void LogFatal(string loggerName, string message)
        {
            Logger(loggerName)?.Fatal(message);
        }

        public static void LogFatalFormat(string loggerName, string msgFormat, params object[] args)
        {
            Logger(loggerName)?.FatalFormat(msgFormat, args);
        }
    }
}
