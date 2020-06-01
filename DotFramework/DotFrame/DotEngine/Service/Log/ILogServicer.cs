namespace DotEngine.Service.Log
{
    public enum LogLevelType
    {
        Debug = 0,
        Info,
        Warning,
        Error,
        Fatal,
    }

    public interface ILogServicer :IServicer
    {
        void Log(LogLevelType levelType, string tag, string message);

        void LogDebug(string tag, string message);
        void LogInfo(string tag, string message);
        void LogWarning(string tag, string message);
        void LogError(string tag, string message);
        void LogFatal(string tag, string message);
    }
}
