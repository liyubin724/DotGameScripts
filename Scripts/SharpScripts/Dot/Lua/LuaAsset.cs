using System;
using UnityEngine;
using XLua;

namespace Dot.Lua
{
    [Serializable]
    public class LuaAsset
    {
        [SerializeField]
        private string scriptFilePath = "";

        public bool Require(LuaEnv luaEnv)
        {
            return LuaRequire.Require(luaEnv, scriptFilePath);
        }

        public LuaTable Instance(LuaEnv luaEnv)
        {
            return LuaRequire.Instance(luaEnv, scriptFilePath);
        }
    }
}
