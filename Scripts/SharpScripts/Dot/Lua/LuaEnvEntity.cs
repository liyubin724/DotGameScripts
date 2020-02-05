using Dot.Log;
using Dot.Timer;
using Dot.Lua.Loader;
using System;
using XLua;
using SystemObject = System.Object;

namespace Dot.Lua
{
    public class LuaEnvEntity
    {
        private static float DEFAULT_ENV_TICK = 60;

        private LuaEnv luaEnv = null;
        internal LuaEnv Lua { get => luaEnv; }

        private LuaTable mgrInLua = null;
        private Action<LuaTable,float> updateAction = null;

        private TimerTaskInfo timerInfo = null;
        private float tickInterval = 0;
        public float TickInterval
        {
            set {
                tickInterval = value;

                if(timerInfo!=null)
                {
                    TimerManager.GetInstance().RemoveTimer(timerInfo);
                    timerInfo = null;
                }

                if(tickInterval>0)
                {
                    TimerManager.GetInstance().AddIntervalTimer(tickInterval, OnTimerTick);
                }
            }
        }
        
        public LuaEnvEntity(string[] scriptPathFormats)
        {
            luaEnv = new LuaEnv();
#if DEBUG
            luaEnv.Global.Set("IsDebug", true);
#endif
            luaEnv.AddBuildin("rapidjson", XLua.LuaDLL.Lua.LoadRapidjson);
            luaEnv.AddBuildin("pb", XLua.LuaDLL.Lua.LoadProtobuf);

            LuaScriptFileLoader.ScriptLoadFromFile(luaEnv, scriptPathFormats);

            TickInterval = DEFAULT_ENV_TICK;
        }

        private void OnTimerTick(SystemObject sysObj)
        {
            if(luaEnv!=null)
            {
                luaEnv.Tick();
            }
        }

        public void DoStart(LuaAsset[] assets, string mgrNameInLua)
        {
            if(assets!=null && assets.Length>0)
            {
                foreach(var asset in assets)
                {
                    if(asset.IsValid())
                    {
                        asset.DoRequire(luaEnv);
                    }else
                    {
                        LogUtil.LogError(typeof(LuaEnvEntity), "LuaEnvEntity::DoStart->param is an unvalid asset");
                    }
                }
            }

            if (!string.IsNullOrEmpty(mgrNameInLua))
            {
                mgrInLua = luaEnv.Global.Get<LuaTable>(mgrNameInLua);

                if (mgrInLua != null)
                {
                    Action<LuaTable> luaMgrStartAction = mgrInLua.Get<Action<LuaTable>>(LuaConfig.START_FUNCTION_NAME);
                    luaMgrStartAction?.Invoke(mgrInLua);
                    luaMgrStartAction = null;

                    updateAction = mgrInLua.Get<Action<LuaTable,float>>(LuaConfig.UPDATE_FUNCTION_NAME);
                }
                else
                {
                    LogUtil.LogError(typeof(LuaEnvEntity), "LuaEnvEntity:DoStart->Bridge Not found.plz require it at first");
                }
            }
        }

        public void DoUpdate(float deltaTime)
        {
            if(mgrInLua!=null && updateAction!=null)
            {
                updateAction.Invoke(mgrInLua, deltaTime);
            }
        }

        public void DoDestroy()
        {
            updateAction = null;

            if(mgrInLua!=null)
            {
                LuaFunction destroyFunc = mgrInLua.Get<LuaFunction>(LuaConfig.DESTROY_FUNCTION_NAME);
                destroyFunc?.Action(mgrInLua);
                destroyFunc.Dispose();

                mgrInLua.Dispose();
                mgrInLua = null;
            }
            luaEnv?.Dispose();
            luaEnv = null;
        }

        public void FullGC()
        {
            luaEnv?.FullGc();
        }
    }
}
