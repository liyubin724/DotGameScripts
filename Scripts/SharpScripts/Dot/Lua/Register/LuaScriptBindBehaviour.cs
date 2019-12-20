using Dot.Log;
using System;
using UnityEngine;
using XLua;

namespace Dot.Lua.Register
{
    public class LuaScriptBindBehaviour : MonoBehaviour
    {
        public LuaEnvType envType = LuaEnvType.Game;
        public LuaAsset luaAsset;

        protected LuaEnv luaEnv;

        public LuaTable ObjTable { get; private set; }

        private bool isInited = false;
        public void InitLua()
        {
            if (isInited)
                return;

            isInited = true;

            luaEnv = LuaManager.GetInstance()[envType];
            if (luaEnv == null)
            {
                LogUtil.LogError(typeof(LuaScriptBindBehaviour), $"LuaRegisterBehaviour::InitLua->LuaEnv is null. envType = {envType}");
                return;
            }

            if (luaAsset != null && luaAsset.IsValid())
            {
                ObjTable = luaAsset.DoRequireAndInstance(luaEnv);
                if (ObjTable != null)
                {
                    ObjTable.Set("gameObject", gameObject);
                    ObjTable.Set("transform", transform);

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
            if (ObjTable != null)
            {
                ObjTable.Get<Action<LuaTable>>(LuaConfig.AWAKE_FUNCTION_NAME)?.Invoke(ObjTable);
            }
        }

        protected virtual void Start()
        {
            if (ObjTable != null)
            {
                ObjTable.Get<Action<LuaTable>>(LuaConfig.START_FUNCTION_NAME)?.Invoke(ObjTable);
            }
        }

        protected virtual void OnEnable()
        {
            if (ObjTable != null)
            {
                ObjTable.Get<Action<LuaTable>>(LuaConfig.ENABLE_FUNCTION_NAME)?.Invoke(ObjTable);
            }
        }

        protected virtual void OnDisable()
        {
            if (ObjTable != null)
            {
                ObjTable.Get<Action<LuaTable>>(LuaConfig.DISABLE_FUNCTION_NAME)?.Invoke(ObjTable);
            }
        }

        protected virtual void OnDestroy()
        {
            if (luaEnv == null || ObjTable == null)
            {
                return;
            }
            ObjTable.Get<Action<LuaTable>>(LuaConfig.DESTROY_FUNCTION_NAME)?.Invoke(ObjTable);

            ObjTable.Dispose();
            ObjTable = null;
            luaEnv = null;
        }
    }
}
