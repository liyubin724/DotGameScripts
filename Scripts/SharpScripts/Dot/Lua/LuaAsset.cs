using Dot.Core.Logger;
using System;
using System.IO;
using UnityEngine;
using XLua;

namespace Dot.Lua
{
    [Serializable]
    public class LuaAsset
    {
        [SerializeField]
        private string m_ScriptName = "";

        [SerializeField]
        private string m_ScriptPath = "";

        public string LuaScriptPath
        {
            set
            {
                m_ScriptPath = value;
                if(!string.IsNullOrEmpty(m_ScriptPath))
                {
                    m_ScriptName = Path.GetFileNameWithoutExtension(m_ScriptPath);
                }else
                {
                    m_ScriptName = null;
                }
            }
        }

        public bool IsValid()
        {
            return !string.IsNullOrEmpty(m_ScriptName) && !string.IsNullOrEmpty(m_ScriptPath);
        }

        public bool DoRequire(LuaEnv luaEnv)
        {
            if(string.IsNullOrEmpty(m_ScriptName) || string.IsNullOrEmpty(m_ScriptPath))
            {
                DebugLogger.LogError(string.Format("LuaAsset::DoRequire->ScriptName or ScriptPath is NULL!"));
                return false;
            }
            if(luaEnv.Global.ContainsKey<string>(m_ScriptName))
            {
                return true;
            }
            luaEnv.DoString(string.Format("require (\"{0}\")", m_ScriptPath));
            return true ;
        }

        public LuaTable DoRequireAndInstance(LuaEnv luaEnv)
        {
            if (string.IsNullOrEmpty(m_ScriptName) || string.IsNullOrEmpty(m_ScriptPath))
            {
                DebugLogger.LogError(string.Format("LuaAsset::GetInstance->scriptName is NULL!"));
                return null;
            }

            LuaTable classTable = luaEnv.Global.Get<LuaTable>(m_ScriptName);
            if (classTable == null)
            {
                luaEnv.DoString(string.Format("require (\"{0}\")", m_ScriptPath));
                classTable = luaEnv.Global.Get<LuaTable>(m_ScriptName);
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
