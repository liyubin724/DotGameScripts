using Dot;
using Dot.Lua;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;

namespace Game
{
    public class GameController : MonoBehaviour
    {
        public LuaAsset[] preloadLuaAssets;
        public LuaEnvType luaEnvType = LuaEnvType.Game;
        public string luaMgrName = "LuaManager";

        private void Awake()
        {
            DotProxy.StartUp();

            string[] luaPathFormat = new string[]
            {
                LuaConfig.DefaultDiskPathFormat,
            };

            LuaManager.GetInstance().NewLuaEnv(luaEnvType, luaPathFormat,preloadLuaAssets, luaMgrName);
        }
    }
}

