using Dot;
using Dot.Asset;
using Dot.Log;
using Dot.Lua;
using UnityEngine;

namespace Game
{
    public class GameController : MonoBehaviour
    {
        public LuaAsset[] preloadLuaAssets;

        public LuaEnvType luaEnvType = LuaEnvType.Game;
        public string luaMgrName = "LuaManager";

        private void Awake()
        {
            DotProxy.Startup((result) => {

                LogUtil.LogInfo("GameController", "GameController::Awake->DotProxy is inited.result = "+result.ToString());
                if(result)
                {
                    AssetLoaderMode loaderMode = AssetLoaderMode.AssetDatabase;
                    string assetRootDir = string.Empty;
#if ASSET_BUNDLE

#if UNITY_EDITOR
                    loaderMode = AssetLoaderMode.AssetBundle;
                    assetRootDir = "E:/WorkSpace/DotGameProject/DotGameClient/AssetConfig/StandaloneWindows64/assetbundles";
#else

#endif

#else

#if UNITY_EDITOR

#else

#endif

#endif
                    AssetManager.GetInstance().InitManager(loaderMode, (isInitSuccess) =>
                    {
                        if (isInitSuccess)
                        {
                            string[] luaPathFormat = new string[]
                            {
                                LuaConfig.DefaultDiskPathFormat,
                            };

                            LuaManager.GetInstance().NewLuaEnv(luaEnvType, luaPathFormat, preloadLuaAssets, luaMgrName);
                        }
                    }, assetRootDir);



                }
            });

            

        }
    }
}

