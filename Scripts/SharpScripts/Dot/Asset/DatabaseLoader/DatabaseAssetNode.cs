#if UNITY_EDITOR
using Dot.Asset.Datas;
using Dot.Log;
using UnityObject = UnityEngine.Object;

namespace Dot.Asset
{
    public class DatabaseAssetNode : AAssetNode
    {
        private UnityObject uObject = null;
        public void SetAsset(UnityObject uObj)
        {
            uObject = uObj;
            State = NodeState.Finished;
        }

        public override UnityEngine.Object GetAsset()
        {
            if(State == NodeState.Finished)
            {
                return uObject;
            }else
            {
                LogUtil.LogError(AssetConst.LOGGER_NAME, "Asset is not loaded");
                return null;
            }
        }

        public override UnityEngine.Object GetInstance()
        {
            if(State == NodeState.Finished && uObject!=null)
            {
                return UnityObject.Instantiate(uObject);
            }else
            {
                LogUtil.LogError(AssetConst.LOGGER_NAME, "State is not finished or object is null");
                return null;
            }
        }

        public override bool IsAlive()
        {
            if(IsNeverDestroy)
            {
                return true;
            }
            return refCount > 0;
        }

        public override bool IsDone()
        {
            return State == NodeState.Finished;
        }
    }
}
#endif