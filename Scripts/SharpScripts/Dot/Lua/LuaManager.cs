using Dot.Log;
using Dot.Util;
using System.Collections.Generic;
using XLua;

namespace Dot.Lua
{
    public enum LuaEnvType
    {
        Update,
        Game,
    }

    public class LuaManager : Singleton<LuaManager>
    {
        private Dictionary<LuaEnvType, LuaEnvEntity> envEntityDic = new Dictionary<LuaEnvType, LuaEnvEntity>();

        public LuaEnv this[LuaEnvType envType]
        {
            get
            {
                if(envEntityDic.TryGetValue(envType,out LuaEnvEntity entity))
                {
                    return entity.LuaEnv;
                }else
                {
                    LogUtil.LogError(typeof(LuaManager), $"LuaManager::this[LuaEnvType]=>LuaEnv not found.EnvType = {envType}");
                    return null;
                }
            }
        }

        public void NewLuaEnv(LuaEnvType envType,string[] scriptPathFormats,LuaAsset[] prerequiredAssets,string mgrNameInLua)
        {
            if(HasLuaEnv(envType))
            {
                LogUtil.LogError(typeof(LuaManager), "LuaManager::NewLuaEnv->LuaEnv has been created.");
                return;
            }

            LuaEnvEntity envEntity = new LuaEnvEntity(scriptPathFormats);
            envEntity.DoStart(prerequiredAssets, mgrNameInLua);

            envEntityDic.Add(envType, envEntity);
        }

        public void DeleteLuaEnv(LuaEnvType envType)
        {
            if (envEntityDic.TryGetValue(envType, out LuaEnvEntity entity))
            {
                envEntityDic.Remove(envType);
                entity.DoDestroy();
            }
        }

        public bool HasLuaEnv(LuaEnvType envType)
        {
            return envEntityDic.ContainsKey(envType);
        }

        public void FullGC()
        {
            foreach(var kvp in envEntityDic)
            {
                if(kvp.Value!=null)
                {
                    kvp.Value.LuaEnv?.FullGc();
                }
            }
        }

        public void FullGC(LuaEnvType envType)
        {
            LuaEnv luaEnv = this[envType];
            if (luaEnv != null)
            {
                luaEnv.FullGc();
            }
        }

        public void DoUpdate(float deltaTime)
        {
            foreach(var kvp in envEntityDic)
            {
                kvp.Value.DoUpdate(deltaTime);
            }
        }
    }
}
