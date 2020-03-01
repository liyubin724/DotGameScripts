using Dot.Log;
using System;
using UnityEngine;
using XLua;

namespace Dot.Lua.Register
{
    public class LuaScriptBindBehaviour : MonoBehaviour
    {
        public LuaAsset luaAsset;
        protected LuaEnv luaEnv;

        public LuaTable ObjTable { get; private set; }

        private bool isInited = false;
        public void InitLua()
        {
            if (isInited)
                return;

            LuaEnv luaEnv = LuaManager.GetInstance().Env;
            if (!luaEnv.IsValid())
            {
                LogUtil.LogError(typeof(LuaScriptBindBehaviour), $"LuaRegisterBehaviour::InitLua->LuaEnv is null.");
                return;
            }

            if (luaAsset != null)
            {
                ObjTable = luaAsset.Instance(luaEnv);
                if (ObjTable != null)
                {
                    ObjTable.Set("gameObject", gameObject);
                    ObjTable.Set("transform", transform);

                    isInited = true;

                    OnInitFinished();
                }
                else
                {
                    LogUtil.LogError(typeof(LuaScriptBindBehaviour), $"LuaRegisterBehaviour::InitLua->objTable is null.");
                    return;
                }
            }
        }

        protected virtual void OnInitFinished()
        {

        }

        protected virtual void Awake()
        {
            InitLua();
            CallAction(LuaConfig.AWAKE_FUNCTION_NAME);
        }

        protected virtual void Start()
        {
            CallAction(LuaConfig.START_FUNCTION_NAME);
        }

        protected virtual void OnEnable()
        {
            CallAction(LuaConfig.ENABLE_FUNCTION_NAME);
        }

        protected virtual void OnDisable()
        {
            CallAction(LuaConfig.DISABLE_FUNCTION_NAME);
        }

        protected virtual void OnDestroy()
        {
            if (!LuaManager.GetInstance().IsValid() || ObjTable == null)
            {
                return;
            }
            CallAction(LuaConfig.DESTROY_FUNCTION_NAME);
            ObjTable.Dispose();
            ObjTable = null;
        }

        public void CallAction(string funcName)
        {
            if (ObjTable != null)
            {
                ObjTable.Get<Action<LuaTable>>(funcName)?.Invoke(ObjTable);
            }
        }

        public void CallAction(string funcName,LuaTable item,int intValue)
        {
            if(ObjTable != null)
            {
                Action<LuaTable,LuaTable, int> action = ObjTable.Get<Action<LuaTable, LuaTable,int>>(funcName);
                if(action !=null)
                {
                    action.Invoke(ObjTable, item,intValue);
                }
            }
        }

        public string CallFunc(string funcName,int intValue)
        {
            if(ObjTable != null)
            {
                Func<LuaTable, int, string> func = ObjTable.Get<Func<LuaTable, int, string>>(funcName);
                if(func !=null)
                {
                    return func(ObjTable, intValue);
                }
            }
            return null;
        }
    }
}
