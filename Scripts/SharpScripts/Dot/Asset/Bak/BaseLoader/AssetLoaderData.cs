﻿using Dot.Pool;
using Priority_Queue;
using SystemObject = System.Object;
using UnityObject = UnityEngine.Object;

namespace Dot.Core.Loader
{
    public class AssetLoaderData : StablePriorityQueueNode,IObjectPoolItem
    {
        public long uniqueID = -1;
        public string[] pathOrAddresses;
        public string[] assetPaths;

        public bool isInstance = false;
        public SystemObject userData;

        public OnAssetLoadComplete completeCallback;
        public OnAssetLoadProgress progressCallback;
        public OnBatchAssetLoadComplete batchCompleteCallback;
        public OnBatchAssetsLoadProgress batchProgressCallback;
        
        private bool[] assetLoadStates;
        internal void InitData()
        {
            assetLoadStates = new bool[assetPaths.Length];
        }

        internal bool GetLoadState(int index) => assetLoadStates[index];
        internal bool SetLoadState(int index) => assetLoadStates[index] = true;

        internal void InvokeComplete(int index,UnityObject uObj)
        {
            string pathOrAddress = pathOrAddresses[index];
            progressCallback?.Invoke(pathOrAddress, 1.0f, userData);
            completeCallback?.Invoke(pathOrAddress, uObj, userData);
        }

        internal void InvokeProgress(int index, float progress) => progressCallback?.Invoke(pathOrAddresses[index], progress, userData);
        internal void InvokeBatchComplete(UnityObject[] uObjs) => batchCompleteCallback?.Invoke(pathOrAddresses, uObjs, userData);
        internal void InvokeBatchProgress(float[] progresses) => batchProgressCallback?.Invoke(pathOrAddresses, progresses, userData);

        internal void CancelLoader()
        {
            completeCallback = null;
            progressCallback = null;
            batchCompleteCallback = null;
            batchProgressCallback = null;
            userData = null;
        }

        public void OnGet() { }

        public void OnRelease()
        {
            CancelLoader();
            uniqueID = -1;
            assetPaths = null;
            isInstance = false;
            assetLoadStates = null;
        }
    }
}
