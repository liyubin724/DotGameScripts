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
        internal string AssetPath { get; set; }
        internal OperationState State { get; set; } = OperationState.None;

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
        internal abstract UnityEngine.Object GetAsset();
        internal abstract float GetProgress();
    }
}
