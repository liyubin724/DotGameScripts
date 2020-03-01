using Dot.Log;
using Dot.Lua.Loader;
using Dot.Timer;
using Dot.Util;
using System;
using XLua;
using SystemObject = System.Object;

namespace Dot.Lua
{
    public class LuaManager : Singleton<LuaManager>
    {
        private static readonly string LUA_PACKAGE_NAME = "Dot";
        private static readonly float DEFAULT_ENV_TICK = 60;

        private LuaEnv luaEnv = null;
        public LuaEnv Env 
        {
            get
            {
                if( luaEnv == null || !luaEnv.IsValid())
                {
                    return null;
                }
                return luaEnv;
            } 
         }

        public bool IsValid()
        {
            return luaEnv != null && luaEnv.IsValid();
        }

        private LuaTable mgrTable = null;
        private Action<LuaTable, float> updateAction = null;

        private TimerTaskInfo timerInfo = null;
        private float tickInterval = 0;
        public float TickInterval
        {
            set
            {
                tickInterval = value;

                if (timerInfo != null)
                {
                    TimerManager.GetInstance().RemoveTimer(timerInfo);
                    timerInfo = null;
                }

                if (tickInterval > 0)
                {
                    timerInfo = TimerManager.GetInstance().AddIntervalTimer(tickInterval, OnTimerTick);
                }
            }
        }

        private void OnTimerTick(SystemObject sysObj)
        {
            if(IsValid())
            {
                luaEnv.Tick();
            }
        }

        public void CreateLuaEnv(string[] scriptPathFormats, LuaAsset[] prerequiredAssets)
        {
            luaEnv = new LuaEnv();
#if DEBUG
            luaEnv.Global.Set("IsDebug", true);
#endif
            luaEnv.AddBuildin("rapidjson", XLua.LuaDLL.Lua.LoadRapidjson);
            luaEnv.AddBuildin("pb", XLua.LuaDLL.Lua.LoadProtobuf);

            LuaScriptFileLoader.ScriptLoadFromFile(luaEnv, scriptPathFormats);

            TickInterval = DEFAULT_ENV_TICK;

            if(prerequiredAssets!=null && prerequiredAssets.Length>0)
            {
                foreach(var asset in prerequiredAssets)
                {
                    RequiredAsset(asset);
                }
            }

            InitManager(LUA_PACKAGE_NAME);
        }

        public void RequiredAsset(LuaAsset luaAsset)
        {
            if(luaAsset!=null && IsValid())
            {
                if (!luaAsset.Require(luaEnv))
                {
                    LogUtil.LogError(typeof(LuaManager), "LuaManager::RequiredAsset->param is an unvalid asset");
                }
            }
        }

        private void InitManager(string mgrName)
        {
            if (!string.IsNullOrEmpty(mgrName))
            {
                mgrTable = luaEnv.Global.Get<LuaTable>(mgrName);

                if (mgrTable != null)
                {
                    Action<LuaTable> luaMgrStartAction = mgrTable.Get<Action<LuaTable>>(LuaConfig.START_FUNCTION_NAME);
                    luaMgrStartAction?.Invoke(mgrTable);

                    updateAction = mgrTable.Get<Action<LuaTable, float>>(LuaConfig.UPDATE_FUNCTION_NAME);
                }
                else
                {
                    LogUtil.LogError(typeof(LuaManager), "LuaManager:InitManager->Bridge Not found.please require it at first");
                }
            }
        }

        public void FullGC()
        {
            if(IsValid())
            {
                luaEnv.FullGc();
            }
        }

        protected override void DoInit()
        {
            base.DoInit();
            UpdateProxy.GetInstance().DoUpdateHandle += DoUpdate;
        }

        private void DoUpdate(float deltaTime)
        {
            if(IsValid() && mgrTable != null) 
            {
                updateAction?.Invoke(mgrTable,deltaTime);
            }
        }

        public override void DoDispose()
        {
            UpdateProxy.GetInstance().DoUpdateHandle -= DoUpdate;

            if (IsValid())
            {
                updateAction = null;
                System.GC.Collect();
                if(mgrTable!=null)
                {
                    mgrTable.Dispose();
                    mgrTable = null;
                }

                luaEnv.Dispose();
                luaEnv = null;
            }

            base.DoDispose();
        }
    }
}
