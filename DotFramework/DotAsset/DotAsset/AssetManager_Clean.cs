﻿using Dot.Asset.Datas;
using Dot.Log;
using Dot.Timer;
using System;

namespace Dot.Asset
{
    public partial class AssetManager
    {
        private TimerTaskInfo autoCleanTimer = null;
        private float autoCleanInterval = 60;
        /// <summary>
        /// 获取和指定清理资源的周期,默认60秒
        /// </summary>
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
                    StartAutoClean();
                }
            }
        }

        private void StartAutoClean()
        {
            if (autoCleanTimer != null)
            {
                StopAutoClean();
            }
            else if (autoCleanInterval > 0)
            {
                autoCleanTimer = TimerManager.GetInstance().AddIntervalTimer(autoCleanInterval, (userData) => assetLoader?.UnloadUnusedAsset());
            }
        }
        private void StopAutoClean()
        {
            if (autoCleanTimer != null)
            {
                TimerManager.GetInstance().RemoveTimer(autoCleanTimer);
                autoCleanTimer = null;
            }
        }

        /// <summary>
        /// 默认情况下资源的清理是基于GC和定时器，通过此接口可以立即深度清理资源
        /// </summary>
        /// <param name="callback">清理完毕后回调</param>
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
