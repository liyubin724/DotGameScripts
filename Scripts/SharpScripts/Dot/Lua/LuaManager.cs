using Dot.Log;
using Dot.Manager;
using XLua;

namespace Dot.Lua
{
    public class LuaManager : BaseSingletonManager<LuaManager>
    {
        internal static readonly string LOGGER_NAME = "LuaEnv";
        private static readonly string LUA_NAME = "Dot";

        private LuaEnvEntity luaEnvEntity = null;
        public LuaEnv LuaEnv { get => luaEnvEntity?.Lua; }

        public void NewLuaEnv(string[] scriptPathFormats, LuaAsset[] prerequiredAssets)
        {
            if(luaEnvEntity!=null)
            {
                LogUtil.LogError(LOGGER_NAME,"lua env has been started");
                return;
            }
            luaEnvEntity = new LuaEnvEntity(scriptPathFormats);
            luaEnvEntity.DoStart(prerequiredAssets, LUA_NAME);
        }

        public void DisposeLuaEnv()
        {
            if(luaEnvEntity != null)
            {
                luaEnvEntity.DoDestroy();
                luaEnvEntity = null;
            }
        }

        public void FullGC()
        {
            if(luaEnvEntity!=null)
            {
                luaEnvEntity.FullGC();
            }
        }

        public override void DoInit()
        {
            base.DoInit();
            BindUpdate(true, false, false);
        }

        protected override void DoUpdate(float deltaTime)
        {
            luaEnvEntity?.DoUpdate(deltaTime);
        }

        public override void DoDispose()
        {
            DisposeLuaEnv();
            base.DoDispose();
        }
    }
}
