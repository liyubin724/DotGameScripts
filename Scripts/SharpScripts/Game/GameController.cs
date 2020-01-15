using Dot;
using Dot.Asset;
using Dot.Dispatch;
using Dot.Log;
using Dot.Lua;
using Game.Dispatch;
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
            DotProxy.Startup();
            EventManager.GetInstance().RegisterEvent(EventConst.PROXY_INIT, OnProxyInitFinish);
        }

        private void OnProxyInitFinish(EventData eventData)
        {
            if(!eventData.GetValue<bool>())
            {
                Debug.LogError("GameController::OnProxyInitFinish->Proxy Init failed");
            }else
            {
                LogUtil.LogInfo(GetType(), "OnProxyInitFinish->init success");

                InitAssetManager();
            }
        }

        private void InitAssetManager()
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
            AssetManager.GetInstance().InitManager(loaderMode, OnAssetMgrInitFinish, assetRootDir);
        }

        private void OnAssetMgrInitFinish(bool result)
        {
            if(result)
            {
                string[] luaPathFormat = new string[]
                    {
                                LuaConfig.DefaultDiskPathFormat,
                    };

                LuaManager.GetInstance().NewLuaEnv(luaEnvType, luaPathFormat, preloadLuaAssets, luaMgrName);

                OnControllerInitFinish();
            }
        }

        private void OnControllerInitFinish()
        {
            EventManager.GetInstance().TriggerEvent(GameEventConst.CONTROLLER_INIT);
        }
    }
}

