using UnityObject = UnityEngine.Object;

namespace Dot.Asset
{
    public enum OperationState
    {
        None = 0,
        Loading,
        Finished,
    }

    public delegate bool GetAssetFilePath(string bundlePath, out string filePath, out ulong offset);

    public abstract class AAsyncOperation
    {
        internal string AssetPath { get; private set; }
        protected GetAssetFilePath getFilePath = null;
        internal OperationState State { get; set; } = OperationState.None;

        protected AAsyncOperation(string assetPath)
        {
            AssetPath = assetPath;
        }

        protected AAsyncOperation(string assetPath, GetAssetFilePath getFilePath) : this(assetPath)
        {
            this.getFilePath = getFilePath;
        }

        internal void DoUpdate()
        {
            if(State == OperationState.None)
            {
                OnOperationStart();
            }else if(State == OperationState.Loading)
            {
                OnOperationLoading();
            }
        }

        protected abstract void OnOperationStart();
        protected abstract void OnOperationLoading();
        protected internal abstract UnityObject GetAsset();
        protected internal abstract float GetProgress();
    }
}
