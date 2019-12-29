namespace Dot.Asset
{
    public enum OperationState
    {
        None = 0,
        Loading,
        Finished,
    }

    public abstract class AAsyncOperation
    {
        protected string assetFullPath = null;
        internal OperationState State { get; set; } = OperationState.None;

        public AAsyncOperation(string assetFullPath)
        {
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
        protected abstract UnityEngine.Object GetAsset();
        protected abstract float GetProgress();
    }
}
