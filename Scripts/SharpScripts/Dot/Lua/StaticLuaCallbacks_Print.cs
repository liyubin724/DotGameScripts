using Dot.Log;
#if USE_UNI_LUA
using LuaAPI = UniLua.Lua;
using RealStatePtr = UniLua.ILuaState;
using LuaCSFunction = UniLua.CSharpFunctionDelegate;
#else
using LuaAPI = XLua.LuaDLL.Lua;
using RealStatePtr = System.IntPtr;
using LuaCSFunction = XLua.LuaDLL.lua_CSFunction;
#endif

namespace XLua
{
    public partial class StaticLuaCallbacks
    {
        [MonoPInvokeCallback(typeof(LuaCSFunction))]
        internal static int PrintLogMessage(RealStatePtr L)
        {
            try
            {
                if (0 != LuaAPI.xlua_getglobal(L, "tostring"))
                {
                    return LuaAPI.luaL_error(L, "can not get tostring in print:");
                }
                int n = LuaAPI.lua_gettop(L);
                if(n<3)
                {
                    return LuaAPI.luaL_error(L, "import generic type need at lease 2 arguments");
                }

                string loggerName = LuaAPI.lua_tostring(L, 1);
                int logType = LuaAPI.xlua_tointeger(L, 2);
                string msg = LuaAPI.lua_tostring(L, 3);
                if(logType == 0)
                {
                    LogUtil.LogInfo(loggerName, msg);
                }else if(logType == 1)
                {
                    LogUtil.LogWarning(loggerName, msg);
                }else
                {
                    LogUtil.LogError(loggerName, msg);
                }
                return 0;
            }
            catch (System.Exception e)
            {
                return LuaAPI.luaL_error(L, "c# exception in print:" + e);
            }
        }
    }
}
