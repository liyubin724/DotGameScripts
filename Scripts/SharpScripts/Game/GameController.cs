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

        private void Awake()
        {
            DotProxy.Startup((result) =>
            {
                if (!result)
                {
                    Debug.LogError("GameController::OnProxyInitFinish->Proxy Init failed");
                }
                else
                {
                    LogUtil.LogInfo(GetType(), "OnProxyInitFinish->init success");
                    InitAssetManager();
                }
            });
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
            AssetManager.GetInstance().InitManager(loaderMode, (result)=>
            {
                if (result)
                {
                    LuaManager.GetInstance().CreateLuaEnv(new string[] { LuaConfig.DefaultDiskPathFormat }, preloadLuaAssets);

                    EventManager.GetInstance().TriggerEvent(GameEventConst.CONTROLLER_INIT);
                }else
                {
                    LogUtil.LogError(GetType(), "AssetManager::InitManager->init Failed");
                }
            }, assetRootDir);
        }
    }
}

