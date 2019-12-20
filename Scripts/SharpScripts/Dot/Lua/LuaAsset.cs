using Dot.Log;
using System;
using UnityEngine;
using XLua;

namespace Dot.Lua
{
    [Serializable]
    public class LuaAsset
    {
        [SerializeField]
        private string scriptFileName = "";
        [SerializeField]
        private string scriptFilePath = "";

        public bool IsValid()
        {
            return !string.IsNullOrEmpty(scriptFileName) && !string.IsNullOrEmpty(scriptFilePath);
        }

        public bool DoRequire(LuaEnv luaEnv)
        {
            if(!IsValid())
            {
                LogUtil.LogError(this,"LuaAsset::DoRequire->ScriptName or ScriptPath is NULL!");
                return false;
            }
            if(luaEnv.Global.ContainsKey<string>(scriptFileName))
            {
                return true;
            }

            luaEnv.DoString(string.Format("require (\"{0}\")", scriptFilePath));

            return true ;
        }

        public LuaTable DoRequireAndInstance(LuaEnv luaEnv)
        {
            if (!IsValid())
            {
                LogUtil.LogError(this, "LuaAsset::GetInstance->ScriptFileName or ScriptFilePath is NULL!");
                return null;
            }

            LuaTable classTable = luaEnv.Global.Get<LuaTable>(scriptFileName);
            if (classTable == null)
            {
                luaEnv.DoString(string.Format("require (\"{0}\")", scriptFilePath));
                classTable = luaEnv.Global.Get<LuaTable>(scriptFileName);
            }
            if (classTable == null)
                return null;

            LuaFunction callFun = classTable.Get<LuaFunction>("__call");
            LuaTable objTable = callFun.Func<LuaTable, LuaTable>(classTable);
            callFun.Dispose();
            callFun = null;
            classTable.Dispose();
            classTable = null;

            return objTable;
        }
    }
}
