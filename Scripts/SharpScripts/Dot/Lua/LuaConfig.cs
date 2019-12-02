using UnityEngine;

namespace Dot.Lua
{
    public static class LuaConfig
    {
        public const string AWAKE_FUNCTION_NAME = "DoAwake";
        public const string ENABLE_FUNCTION_NAME = "DoEnable";
        public const string START_FUNCTION_NAME = "DoStart";
        public const string DISABLE_FUNCTION_NAME = "DoDisable";
        public const string UPDATE_FUNCTION_NAME = "DoUpdate";
        public const string DESTROY_FUNCTION_NAME = "DoDestroy";

        public static string LuaAssetDirPath = "Assets/Scripts/DotLua/";
        public static string LuaAssetExtension = ".txt";

        public static string LuaDiskDirPath
        {
            get
            {
                return Application.dataPath + LuaAssetDirPath.Substring("Assets".Length);
            }
        }

        public static string LuaPathFormat
        {
            get
            {
                return $"{LuaDiskDirPath}{{0}}{LuaAssetExtension}";
            }
        }
    }
}
