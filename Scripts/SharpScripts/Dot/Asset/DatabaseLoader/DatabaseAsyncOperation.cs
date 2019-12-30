#if UNITY_EDITOR
using UnityEditor;

namespace Dot.Asset
{
    public class DatabaseAsyncOperation : AAsyncOperation
    {
        internal override UnityEngine.Object GetAsset()
        {
            return AssetDatabase.LoadAssetAtPath(AssetPath, typeof(UnityEngine.Object));
        }

        internal override float GetProgress()
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