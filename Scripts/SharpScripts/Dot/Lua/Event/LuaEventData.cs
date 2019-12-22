using Dot.Log;
using Dot.Lua.Register;
using XLua;
using SystemObject = System.Object;
using System;

namespace Dot.Lua.Event
{
    [Serializable]
    public class LuaEventData
    {
        public LuaScriptBindBehaviour bindBehaviour = null;
        public string funcName = string.Empty;
        public LuaEventParam[] eventParams = new LuaEventParam[0];

        public void Invoke()
        {
            if(bindBehaviour == null)
            {
                LogUtil.LogError(GetType(), "bindBehaviour is null");
                return;
            }
            if(string.IsNullOrEmpty(funcName))
            {
                LogUtil.LogError(GetType(), "funcName is null");
                return;
            }

            LuaFunction luaFunc = bindBehaviour.ObjTable.Get<LuaFunction>(funcName);
            if(luaFunc == null)
            {
                LogUtil.LogError(GetType(), "lua func is not found");
                return;
            }
            if(eventParams == null || eventParams.Length == 0)
            {
                luaFunc.Action<LuaTable>(bindBehaviour.ObjTable);
            }else
            {
                SystemObject[] values = new SystemObject[eventParams.Length + 1];
                values[0] = bindBehaviour.ObjTable;
                for(int i =0;i<eventParams.Length;++i)
                {
                    values[i + 1] = eventParams[i].GetValue();
                }
                luaFunc.ActionParams(values);
            }
            luaFunc.Dispose();
        }
    }
}
