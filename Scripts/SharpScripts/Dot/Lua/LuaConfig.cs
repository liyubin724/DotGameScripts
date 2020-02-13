using UnityEngine;

namespace Dot.Lua
{
    public static class LuaConfig
    {
        public const string LOGGER_NAME = "Lua";

        public const string AWAKE_FUNCTION_NAME = "DoAwake";
        public const string ENABLE_FUNCTION_NAME = "DoEnable";
        public const string START_FUNCTION_NAME = "DoStart";
        public const string DISABLE_FUNCTION_NAME = "DoDisable";
        public const string UPDATE_FUNCTION_NAME = "DoUpdate";
        public const string DESTROY_FUNCTION_NAME = "DoDestroy";

        public const string DEFAULT_ASSET_DIR = "Assets/Scripts/LuaScripts";
        public const string DEFAULT_SCRIPT_EXTENSION = ".txt";
        public const string DEFAULT_ASSET_PATH_FORMAT = DEFAULT_ASSET_DIR + "/{0}" + DEFAULT_SCRIPT_EXTENSION;

        public static string LuaDiskDirPath
        {
            get
            {
                return Application.dataPath + DEFAULT_ASSET_DIR.Substring("Assets".Length);
            }
        }

        public static string DefaultDiskPathFormat
        {
            get
            {
                return $"{LuaDiskDirPath}/{{0}}{DEFAULT_SCRIPT_EXTENSION}";
            }
        }
    }
}
