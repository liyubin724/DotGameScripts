using Dot;
using Dot.Asset;
using Dot.Lua;
using UnityEngine;

namespace Game
{
    public class GameController : MonoBehaviour
    {
        public LuaAsset[] preloadLuaAssets;
        public LuaEnvType luaEnvType = LuaEnvType.Game;
        public string luaMgrName = "LuaManager";

        public TextAsset editorLogConfig = null;
        public TextAsset runtimeLogConfig = null;

        private void Awake()
        {
            DotProxy.StartUp();

#if UNITY_EDITOR
            if (editorLogConfig != null)
            {
                DotProxy.proxy.InitLog(editorLogConfig.text);
            }
#else
            if(runtimeLogConfig!=null)
            {
                DotProxy.proxy.InitLog(runtimeLogConfig.text);
            }
#endif

            AssetManager.GetInstance().InitManager(AssetLoaderMode.AssetDatabase, (result) =>
            {
                if (result)
                {
                    string[] luaPathFormat = new string[]
                    {
                        LuaConfig.DefaultDiskPathFormat,
                    };

                    LuaManager.GetInstance().NewLuaEnv(luaEnvType, luaPathFormat, preloadLuaAssets, luaMgrName);
                }else
                {

                }
            });

        }
    }
}

