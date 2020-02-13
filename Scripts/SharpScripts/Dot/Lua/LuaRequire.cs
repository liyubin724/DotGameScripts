using Dot.Log;
using XLua;

namespace Dot.Lua
{
    public static class LuaRequire
    {
        public static bool Require(LuaEnv luaEnv,string script)
        {
            if(luaEnv == null)
            {
                LogUtil.LogError(LuaConfig.LOGGER_NAME, "luaEnv is null");
                return false;
            }
            if(string.IsNullOrEmpty(script))
            {
                LogUtil.LogError(LuaConfig.LOGGER_NAME, "script is empty");
                return false;
            }
            string scriptName = GetScriptName(script);
            if(string.IsNullOrEmpty(scriptName))
            {
                LogUtil.LogError(LuaConfig.LOGGER_NAME, "scriptName is empty");
                return false;
            }

            if (!luaEnv.Global.ContainsKey(scriptName))
            {
                luaEnv.DoString(string.Format("require (\"{0}\")", script));
            }

            return true;
        }

        public static LuaTable Instance(LuaEnv luaEnv, string script)
        {
            if(!Require(luaEnv,script))
            {
                return null;
            }

            string scriptName = GetScriptName(script);

            LuaTable classTable = luaEnv.Global.Get<LuaTable>(scriptName);
            LuaFunction callFunc = classTable.Get<LuaFunction>("__call");
            LuaTable table = callFunc.Func<LuaTable, LuaTable>(classTable);
            callFunc.Dispose();
            classTable.Dispose();

            return table;
        }

        public static string GetScriptName(string script)
        {
            if (string.IsNullOrEmpty(script))
            {
                LogUtil.LogError(LuaConfig.LOGGER_NAME, "script is empty");
                return null;
            }

            string scriptName = script;
            int index = script.LastIndexOf("/");
            if (index > 0)
            {
                scriptName = script.Substring(index + 1);
            }
            return scriptName;
        }
    }
}
