#if UNITY_EDITOR
using UnityEditor;

namespace Dot.Asset
{
    public class DatabaseAsyncOperation : AAsyncOperation
    {
        public DatabaseAsyncOperation(string assetFullPath) : base(assetFullPath)
        {
        }

        protected override UnityEngine.Object GetAsset()
        {
            return AssetDatabase.LoadAssetAtPath(assetFullPath, typeof(UnityEngine.Object));
        }

        protected override float GetProgress()
        {
            if(State == OperationState.Finished)
            {
                return 1.0f;
            }
            return 0.0f;
        }

        protected override void OnOperationLoading()
        {
            State = OperationState.Finished;
        }

        protected override void OnOperationStart()
        {
            State = OperationState.Loading;
        }
    }
}
#endif 