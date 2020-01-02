using Dot.Asset.Datas;
using Dot.Generic;
using Dot.Log;
using Dot.Pool;
using Priority_Queue;
using System;
using System.Collections.Generic;
using UnityEngine;
using SystemObject = System.Object;

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
        private int maxLoadingCount = 5;

        protected AssetLoaderState State { get; set; }
        protected AssetAddressConfig addressConfig = null;

        internal void Initialize(Action<bool> callback,int maxCount,string assetDir)
        {
            initCallback = callback;
            maxLoadingCount = maxCount;
            State = AssetLoaderState.Initing;
        }
        protected abstract void DoInitUpdate();

        internal AssetHandler LoadBatchAssetAsync(string[] addresses,
            bool isInstance,
            OnAssetLoadComplete complete,
            OnBatchAssetLoadComplete batchComplete,
            OnAssetLoadProgress progress,
            OnBatchAssetsLoadProgress batchProgress,
            AssetLoaderPriority priority,
            SystemObject userData)
        {
            AssetHandler handler = new AssetHandler();

            AssetLoaderData data = dataPool.Get();
            data.handler = handler;
            data.addresses = addresses;
            data.isInstance = isInstance;
            data.complete = complete;
            data.progress = progress;
            data.batchComplete = batchComplete;
            data.batchProgress = batchProgress;
            data.userData = userData;

            if (dataWaitingQueue.Count >= dataWaitingQueue.MaxSize)
            {
                dataWaitingQueue.Resize(dataWaitingQueue.MaxSize * 2);
            }
            dataWaitingQueue.Enqueue(data, (float)priority);
            data.State = DataState.Waiting;

            return handler;
        }

        internal void DoUpdate(float deltaTime)
        {
            if(State == AssetLoaderState.Initing)
            {
                DoInitUpdate();
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
           while(dataWaitingQueue.Count>0 && operations.Count<maxLoadingCount)
            {
                AssetLoaderData data = dataWaitingQueue.Dequeue();
                if(!FindPathAndCheckData(data))
                {
                    data.State = DataState.Error;
                    dataPool.Release(data);
                }

                StartLoadingData(data);
                data.State = DataState.Loading;
                dataLoadingList.Add(data);
            }
        }

        private List<string> tempPathList = new List<string>();
        private bool FindPathAndCheckData(AssetLoaderData data)
        {
            if(data.addresses == null || data.addresses.Length == 0)
            {
                LogUtil.LogError(AssetConst.LOGGER_NAME, "Addresses is null");
                return false;
            }

            if(addressConfig == null)
            {
                LogUtil.LogError(AssetConst.LOGGER_NAME, "addressConfigs is null");
                return false;
            }

            foreach(var address in data.addresses)
            {
                string assetPath = addressConfig.GetPathByAddress(address);
                if(string.IsNullOrEmpty(assetPath))
                {
                    tempPathList.Clear();
                    return false;
                }else
                {
                    tempPathList.Add(assetPath);
                }
            }
            data.paths = tempPathList.ToArray();
            tempPathList.Clear();
            return true;
        }

        protected abstract void StartLoadingData(AssetLoaderData data);

        private void DoAsyncOperationUpdate()
        {
            if(operations.Count>0)
            {
                int index = 0;
                while(operations.Count>index && index<maxLoadingCount)
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

                    data.UpdateDataState();
                    if(data.State>= DataState.Finished)
                    {
                        dataLoadingList.RemoveAt(i);
                        dataPool.Release(data);
                    }
                }
            }
        }

        protected abstract void OnDataUpdate(AssetLoaderData data);

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
    }
}
