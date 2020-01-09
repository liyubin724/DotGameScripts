using Dot.Asset.Datas;
using Dot.Generic;
using Dot.Log;
using Dot.Pool;
using Dot.Timer;
using Priority_Queue;
using System;
using System.Collections.Generic;
using UnityEngine;
using SystemObject = System.Object;
using UnityObject = UnityEngine.Object;

namespace Dot.Asset
{
    public enum AssetLoaderState
    {
        None = 0,
        Initing ,
        Error,
        Running,
    }

    public abstract class AAssetLoader
    {
        protected ObjectPool<AssetLoaderData> dataPool = new ObjectPool<AssetLoaderData>(5);

        protected StablePriorityQueue<AssetLoaderData> dataWaitingQueue = new StablePriorityQueue<AssetLoaderData>(10);
        protected List<AssetLoaderData> dataLoadingList = new List<AssetLoaderData>();
        protected ListDictionary<string, AAsyncOperation> operations = new ListDictionary<string, AAsyncOperation>();

        protected Dictionary<string, AAssetNode> assetNodeDic = new Dictionary<string, AAssetNode>();

        protected Action<bool> initCallback = null;
        protected string assetRootDir = string.Empty;
        public int MaxLoadingCount { get; set; } = 6;

        private TimerTaskInfo autoCleanTimer = null;
        private float autoCleanInterval = 60;
        public float AutoCleanInterval
        {
            set
            {
                if(autoCleanInterval != value && value >=0)
                {
                    autoCleanInterval = value;
                    StartAutoCleanTimer();
                }
            }
        }

        protected AssetLoaderState State { get; set; }
        protected AssetAddressConfig addressConfig = null;

        internal void Initialize(Action<bool> callback,string assetDir)
        {
            initCallback = callback;
            assetRootDir = assetDir;

            State = AssetLoaderState.Initing;
        }
        protected abstract void DoInitUpdate();

        internal AssetHandler LoadBatchAssetAsync(string label,string[] addresses,
            bool isInstance,
            OnAssetLoadComplete complete,
            OnBatchAssetLoadComplete batchComplete,
            OnAssetLoadProgress progress,
            OnBatchAssetsLoadProgress batchProgress,
            AssetLoaderPriority priority,
            SystemObject userData)
        {
            if(!string.IsNullOrEmpty(label) && addresses == null)
            {
                addresses = addressConfig.GetAddressesByLabel(label);
            }

            if (addresses == null || addresses.Length == 0)
            {
                LogUtil.LogError(AssetConst.LOGGER_NAME, "AAssetLoader::LoadBatchAssetAsync->addresses is null");
                return null;
            }

            string[] paths = addressConfig.GetPathsByAddresses(addresses);
            if(paths == null || paths.Length == 0)
            {
                LogUtil.LogError(AssetConst.LOGGER_NAME, "AssetLoader::LoadBatchAssetAsync->paths is null");
                return null;
            }

            AssetLoaderData data = dataPool.Get();
            data.InitData(label, addresses, paths, isInstance, complete, progress, batchComplete, batchProgress, userData);

            if (dataWaitingQueue.Count >= dataWaitingQueue.MaxSize)
            {
                dataWaitingQueue.Resize(dataWaitingQueue.MaxSize * 2);
            }
            dataWaitingQueue.Enqueue(data, (float)priority);
            data.State = DataState.Waiting;

            return data.Handler;
        }

        internal void DoUpdate(float deltaTime)
        {
            if(State == AssetLoaderState.Initing)
            {
                DoInitUpdate();

                if(State == AssetLoaderState.Running)
                {
                    initCallback?.Invoke(true);

                    StartAutoCleanTimer();

                }else if(State == AssetLoaderState.Error)
                {
                    initCallback?.Invoke(false);
                }
                return;
            }else if(State!= AssetLoaderState.Running)
            {
                LogUtil.LogError(AssetConst.LOGGER_NAME, "Init Failed");
                return;
            }

            DoWaitingDataUpdate();
            DoAsyncOperationUpdate();
            DoLoadingDataUpdate();
            DoUnloadUnsedAssetUpdate();
        }

        private void DoWaitingDataUpdate()
        {
           while(dataWaitingQueue.Count>0 && operations.Count<MaxLoadingCount)
            {
                AssetLoaderData data = dataWaitingQueue.Dequeue();
                StartLoadingData(data);
                data.State = DataState.Loading;
                dataLoadingList.Add(data);
            }
        }

        protected abstract void StartLoadingData(AssetLoaderData data);

        private void DoAsyncOperationUpdate()
        {
            if(operations.Count>0)
            {
                int index = 0;
                while(operations.Count>index && index<MaxLoadingCount)
                {
                    AAsyncOperation operation = operations.GetByIndex(index);
                    operation.DoUpdate();

                    if(operation.State >= OperationState.Finished)
                    {
                        operations.DeleteByIndex(index);
                        OnOperationFinished(operation);
                    }else
                    {
                        ++index;
                    }
                }
            }
        }

        protected abstract void OnOperationFinished(AAsyncOperation operation);

        private void DoLoadingDataUpdate()
        {
            if(dataLoadingList.Count>0)
            {
                for(int i = dataLoadingList.Count-1;i>=0;--i)
                {
                    AssetLoaderData data = dataLoadingList[i];
                    OnDataUpdate(data);
                    if(data.State>= DataState.Finished)
                    {
                        dataLoadingList.RemoveAt(i);
                        dataPool.Release(data);
                    }
                }
            }
        }

        protected abstract void OnDataUpdate(AssetLoaderData data);

        internal void UnloadAssetAsync(AssetHandler handler, bool destroyIfIsInstnace)
        {
            if(dataWaitingQueue.Count>0)
            {
                foreach(var data in dataWaitingQueue)
                {
                    if(data.Handler == handler)
                    {
                        dataWaitingQueue.Remove(data);
                        dataPool.Release(data);
                        return;
                    }
                }
            }

            if(dataLoadingList.Count>0)
            {
                foreach(var data in dataLoadingList)
                {
                    if(data.Handler == handler)
                    {
                        data.DoCancel(destroyIfIsInstnace);
                        return;
                    }
                }
            }
        }

        protected internal abstract UnityObject InstantiateAsset(string address, UnityObject asset);

        private void StartAutoCleanTimer()
        {
            if(autoCleanTimer!=null)
            {
                TimerManager.GetInstance().RemoveTimer(autoCleanTimer);
                autoCleanTimer = null;
            }
            if(autoCleanInterval>0)
            {
                autoCleanTimer = TimerManager.GetInstance().AddIntervalTimer(autoCleanInterval, AutoCleanUnusedAsset);
            }
        }

        private void AutoCleanUnusedAsset(SystemObject userData)
        {
            OnUnloadUnusedAsset();
        }

        private Action unloadUnusedCallback = null;
        private AsyncOperation unloadUnusedOperation = null;
        public void UnloadUnusedAsset(Action callback)
        {
            if(unloadUnusedCallback!=null)
            {
                LogUtil.LogError(AssetConst.LOGGER_NAME, "UnloadUnusedAsset is running!!");
                return;
            }

            unloadUnusedCallback = callback;

            OnUnloadUnusedAsset();
            
            GC.Collect();
            GC.Collect();
            unloadUnusedOperation = Resources.UnloadUnusedAssets();
        }

        protected abstract void OnUnloadUnusedAsset();

        private void DoUnloadUnsedAssetUpdate()
        {
            if(unloadUnusedOperation!=null && unloadUnusedOperation.isDone)
            {
                unloadUnusedOperation = null;
                unloadUnusedCallback?.Invoke();
                unloadUnusedCallback = null;
            }
        }

        internal virtual void DoDispose()
        {
            if (autoCleanTimer != null)
            {
                TimerManager.GetInstance().RemoveTimer(autoCleanTimer);
                autoCleanTimer = null;
            }
        }
    }
}
