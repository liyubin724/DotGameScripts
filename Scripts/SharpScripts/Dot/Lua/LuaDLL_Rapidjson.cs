using System;
using System.Runtime.InteropServices;

namespace XLua.LuaDLL
{
    public partial class Lua
    {
        [DllImport(LUADLL,CallingConvention = CallingConvention.Cdecl)]
        public static extern int luaopen_rapidjson(IntPtr L);

        [MonoPInvokeCallback(typeof(LuaDLL.lua_CSFunction))]
        public static int LoadRapidjson(IntPtr L)
        {
            return luaopen_rapidjson(L);
        }
    }
}
