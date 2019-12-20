using log4net;
using log4net.Config;
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
        public static void LogError(object senderObj, string message)
        {
            LogError(senderObj.GetType().Name, message);
        }

        public static void LogError(string loggerName, string message)
        {
            Logger(loggerName)?.Error(message);
        }

        public static void LogErrorFormat(string loggerName, string msgFormat, params object[] args)
        {
            Logger(loggerName)?.ErrorFormat(loggerName, msgFormat, args);
        }
        public static void LogWarning(object senderObj, string message)
        {
            LogWarning(senderObj.GetType().Name, message);
        }

        public static void LogWarning(string loggerName, string message)
        {
            Logger(loggerName)?.Warn(message);
        }

        public static void LogWarningFormat(string loggerName, string msgFormat, params object[] args)
        {
            Logger(loggerName)?.WarnFormat(loggerName, msgFormat, args);
        }
        public static void Log(object senderObj, string message)
        {
            Log(senderObj.GetType().Name, message);
        }

        public static void Log(string loggerName, string message)
        {
            Logger(loggerName)?.Info(message);
        }

        public static void LogFormat(string loggerName, string msgFormat, params object[] args)
        {
            Logger(loggerName)?.InfoFormat(msgFormat, args);
        }
    }
}
