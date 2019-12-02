using Dot.Core.Logger;
using Dot.Lua.Loader;
using System;
using XLua;

namespace Dot.Lua
{
    public class LuaEnvEntity
    {
        private LuaEnv luaEnv = null;

        private LuaTable mgrInLua = null;
        private Action<LuaTable,float> updateAction = null;

        public LuaEnv LuaEnv { get => luaEnv; }
        
        public LuaEnvEntity(string[] scriptPathFormats)
        {
            luaEnv = new LuaEnv();
#if DEBUG
            luaEnv.Global.Set("IsDebug", true);
#endif

            LuaScriptFileLoader.ScriptLoadFromFile(luaEnv, scriptPathFormats);
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
                        DebugLogger.LogError("LuaEnvEntity::DoStart->param is an unvalid asset");
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
                    DebugLogger.LogError("LuaEnvEntity:DoStart->Bridge Not found.plz require it at first");
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
                Action<LuaTable> destroyAction = mgrInLua.Get<Action<LuaTable>>(LuaConfig.DESTROY_FUNCTION_NAME);
                destroyAction?.Invoke(mgrInLua);
                destroyAction = null;

                mgrInLua.Dispose();
                mgrInLua = null;
            }
            luaEnv.Dispose();
            luaEnv = null;
        }
    }
}
