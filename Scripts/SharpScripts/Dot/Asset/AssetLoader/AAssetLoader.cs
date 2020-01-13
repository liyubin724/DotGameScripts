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

        protected AssetLoaderState State { get; set; }
        protected AssetAddressConfig addressConfig = null;
        public string GetAssetPathByAddress(string address)
        {
            if(addressConfig!=null)
            {
                return addressConfig.GetPathByAddress(address);
            }
            return null;
        }

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
                LogUtil.LogInfo(AssetConst.LOGGER_DEBUG_NAME, $"AssetLoader::LoadBatchAssetAsync->Load asset by label.label = {label},addresses = {string.Join(",",addresses)}");
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
            }else
            {
                LogUtil.LogInfo(AssetConst.LOGGER_DEBUG_NAME, $"AssetLoader::LoadBatchAssetAsync->find assetPath by address.addresses = {string.Join(",", addresses)},path = {string.Join(",",paths)}");
            }

            AssetLoaderData data = dataPool.Get();
            data.InitData(label, addresses, paths, isInstance, complete, progress, batchComplete, batchProgress, userData);

            if (dataWaitingQueue.Count >= dataWaitingQueue.MaxSize)
            {
                dataWaitingQueue.Resize(dataWaitingQueue.MaxSize * 2);
                LogUtil.LogInfo(AssetConst.LOGGER_DEBUG_NAME, "AssetLoader::LoadBatchAssetAsync->Reset the queue size.");
            }
            dataWaitingQueue.Enqueue(data, (float)priority);
            data.State = AssetLoaderDataState.Waiting;

            return data.Handler;
        }

        internal void DoUpdate(float deltaTime)
        {
            if(State == AssetLoaderState.Initing)
            {
                LogUtil.LogInfo(AssetConst.LOGGER_DEBUG_NAME, "AssetLoader::DoUpdate->Update to Init Loader.");

                DoInitUpdate();

                if(State == AssetLoaderState.Running)
                {
                    LogUtil.LogInfo(AssetConst.LOGGER_DEBUG_NAME, "AssetLoader::DoUpdate->Loader init success.");

                    initCallback?.Invoke(true);
                }else if(State == AssetLoaderState.Error)
                {
                    LogUtil.LogError(AssetConst.LOGGER_NAME, "AssetLoader::DoUpdate->Loader init failed.");

                    initCallback?.Invoke(false);
                }
                return;
            }else if(State!= AssetLoaderState.Running)
            {
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
                data.State = AssetLoaderDataState.Loading;
                dataLoadingList.Add(data);

                LogUtil.LogInfo(AssetConst.LOGGER_DEBUG_NAME, $"AssetLoader::DoWaitingDataUpdate->Start Load Data.data = {data}");
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
                    if(data.State>= AssetLoaderDataState.Finished)
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

        internal void UnloadAsset(string address,bool isForce)
        {

        }

        protected internal abstract UnityObject InstantiateAsset(string address, UnityObject asset);

        private Action unloadUnusedCallback = null;
        private AsyncOperation unloadUnusedOperation = null;
        internal void DeepUnloadUnusedAsset(Action callback)
        {
            if(unloadUnusedCallback!=null)
            {
                LogUtil.LogError(AssetConst.LOGGER_NAME, "UnloadUnusedAsset is running!!");
                return;
            }

            unloadUnusedCallback = callback;

            UnloadUnusedAsset();
            
            GC.Collect();
            GC.Collect();
            unloadUnusedOperation = Resources.UnloadUnusedAssets();
        }

        protected internal abstract void UnloadUnusedAsset();

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
            
        }
    }
}
