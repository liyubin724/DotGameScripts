using Dot.Pool;
using Priority_Queue;
using SystemObject = System.Object;
using UnityObject = UnityEngine.Object;

namespace Dot.Asset
{
    public enum AssetLoaderDataState
    {
        None = 0,
        Waiting,
        Loading,
        Finished,
        Canceled,
        Error,
    }

    public class AssetLoaderData : StablePriorityQueueNode, IObjectPoolItem
    {
        private string label = string.Empty;
        private string[] addresses = new string[0];
        private string[] paths = new string[0];
        private bool isInstance = false;
        private OnAssetLoadComplete completeCallback;
        private OnAssetLoadProgress progressCallback;
        private OnBatchAssetLoadComplete batchCompleteCallback;
        private OnBatchAssetsLoadProgress batchProgressCallback;
        private SystemObject userData = null;

        private AssetHandler handler = null;
        internal AssetHandler Handler { get => handler; }
        internal AssetLoaderDataState State { get; set; }

        public string[] Paths { get => paths; }

        public void InitData(string label,string[] addresses,string[] paths,bool isInstance,
            OnAssetLoadComplete complete,OnAssetLoadProgress progress,
            OnBatchAssetLoadComplete batchComplete,OnBatchAssetsLoadProgress batchProgress,
            SystemObject userData)
        {
            this.addresses = addresses;
            this.paths = paths;
            this.isInstance = isInstance;
            this.completeCallback = complete;
            this.progressCallback = progress;
            this.batchCompleteCallback = batchComplete;
            this.batchProgressCallback = batchProgress;
            this.userData = userData;

            handler = new AssetHandler(label, addresses, userData);
        }
        
        internal void DoComplete(int index,AAssetNode assetNode)
        {
            string path = paths[index];
            if(!string.IsNullOrEmpty(path))
            {
                paths[index] = null;

                UnityObject uObj;
                if (isInstance)
                {
                    uObj = assetNode.GetInstance();
                }else
                {
                    uObj = assetNode.GetAsset();
                }
                handler.UObjects[index] = uObj;

                handler.IsDone = true;

                progressCallback?.Invoke(addresses[index], 1.0f, userData);
                completeCallback?.Invoke(addresses[index], uObj, userData);
            }
        }

        private bool isProgressChanged = false;
        internal void DoProgress(int index,float progress)
        {
            if(progress!=handler.Progresses[index])
            {
                isProgressChanged = true;

                handler.Progresses[index] = progress;
                progressCallback?.Invoke(addresses[index], progress,userData);
            }
        }

        internal void DoBatchComplete()
        {
            batchProgressCallback?.Invoke(addresses, handler.Progresses, userData);
            batchCompleteCallback?.Invoke(addresses, handler.UObjects, userData);
            State = AssetLoaderDataState.Finished;
        }

        internal void DoBatchProgress()
        {
            if(isProgressChanged)
            {
                batchProgressCallback?.Invoke(addresses, handler.Progresses, userData);
                isProgressChanged = false;
            }
        }

        internal void DoCancel(bool destroyIfIsInstnace)
        {
            handler.DoCancel(isInstance, destroyIfIsInstnace);
            handler = null;

            completeCallback = null;
            progressCallback = null;
            batchCompleteCallback = null;
            batchProgressCallback = null;
            userData = null;

            State = AssetLoaderDataState.Canceled;
        }

        public void OnGet()
        {
        }

        public void OnRelease()
        {
            label = string.Empty;
            addresses = new string[0];
            paths = new string[0];
            isInstance = false;
            completeCallback = null;
            progressCallback = null;
            batchCompleteCallback = null;
            batchProgressCallback = null;
            userData = null;

            handler = null;

            State = AssetLoaderDataState.None;
        }
    }
}
