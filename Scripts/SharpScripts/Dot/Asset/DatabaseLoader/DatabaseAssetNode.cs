#if UNITY_EDITOR
using Dot.Asset.Datas;
using Dot.Log;
using UnityObject = UnityEngine.Object;

namespace Dot.Asset
{
    public class DatabaseAssetNode : AAssetNode
    {
        private UnityObject uObject = null;
        private bool isSetAsset = false;
        public void SetAsset(UnityObject uObj)
        {
            uObject = uObj;
            isSetAsset = true;
        }

        public override UnityEngine.Object GetAsset()
        {
            if(uObject!=null)
            {
                return uObject;
            }else
            {
                LogUtil.LogError(AssetConst.LOGGER_NAME, "Asset is null");
                return null;
            }
        }

        public override UnityEngine.Object GetInstance()
        {
            if(uObject!=null)
            {
                return UnityObject.Instantiate(uObject);
            }else
            {
                LogUtil.LogError(AssetConst.LOGGER_NAME, "State is not finished or object is null");
                return null;
            }
        }

        public override UnityObject GetInstance(UnityObject uObj)
        {
            if (uObj != null)
            {
                return UnityObject.Instantiate(uObj);
            }
            return null;
        }

        public override bool IsAlive()
        {
            if (IsNeverDestroy)
            {
                return true;
            }
            return refCount > 0;
        }

        public override bool IsDone()
        {
            return isSetAsset;
        }

        public override void Unload(bool isForce)
        {
            uObject = null;
            isSetAsset = false;
        }
    }
}
#endif