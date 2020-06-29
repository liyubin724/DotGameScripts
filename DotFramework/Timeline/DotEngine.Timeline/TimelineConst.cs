using System.Collections.Generic;

namespace DotEngine.Timeline
{
    public static class TimelineConst
    {
        public const string LOGGER_NAME = "Timeline";

        //Error for factory
        public const int FACTORY_POOL_HAS_BEEN_ADDED_ERROR = 1;
        public const int FACTORY_POOL_NOT_FOUND_ERROR = 2;



        private static Dictionary<int, string> errorMessageDic = new Dictionary<int, string>();

        static TimelineConst()
        {
        }

        public static string GetErrorMessage(int error,params object[] values)
        {
            if (errorMessageDic.TryGetValue(error, out string message))
            {
                if(values!=null && values.Length>0)
                {
                    return string.Format($"[{error}]    {message}", values);
                }else
                {
                    return $"[{error}]    {message}";
                }
            }
            return $"[{error}]    Empty message";
        }

    }
}
