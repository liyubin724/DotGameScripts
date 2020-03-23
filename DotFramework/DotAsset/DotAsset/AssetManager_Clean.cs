using Dot.Asset.Datas;
using Dot.Log;
using Dot.Timer;
using System;

namespace Dot.Asset
{
    public partial class AssetManager
    {
        private TimerTaskInfo autoCleanTimer = null;
        private float autoCleanInterval = 60;
        public float AutoCleanInterval
        {
            get
            {
                return autoCleanInterval;
            }
            set
            {
                if (autoCleanInterval != value && value >= 0)
                {
                    autoCleanInterval = value;
                    StopAutoClean();
                    StartAutoClean();
                }
            }
        }

        public void StartAutoClean()
        {
            if(autoCleanTimer == null && autoCleanInterval>0)
            {
                autoCleanTimer = TimerManager.GetInstance().AddIntervalTimer(autoCleanInterval, (userData)=>assetLoader?.UnloadUnusedAsset());
            }
        }

        public void StopAutoClean()
        {
            if (autoCleanTimer != null)
            {
                TimerManager.GetInstance().RemoveTimer(autoCleanTimer);
                autoCleanTimer = null;
            }
        }

        public void UnloadUnusedAsset(Action callback = null)
        {
            if (assetLoader == null)
            {
                LogUtil.LogError(AssetConst.LOGGER_NAME, "AssetManager::UnloadUnusedAsset->assetLoader is Null");
                return;
            }

            assetLoader.DeepUnloadUnusedAsset(callback);
        }

        private void DoDispose_Clean()
        {
            StopAutoClean();
        }
    }
}
