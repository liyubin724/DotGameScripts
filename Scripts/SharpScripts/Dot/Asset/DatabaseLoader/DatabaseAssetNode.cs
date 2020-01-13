#if UNITY_EDITOR
using Dot.Asset.Datas;
using Dot.Log;
using UnityObject = UnityEngine.Object;

namespace Dot.Asset
{
    internal class DatabaseAssetNode : AAssetNode
    {
        private UnityObject uObject = null;
        private bool isSetAsset = false;
        internal void SetAsset(UnityObject uObj)
        {
            uObject = uObj;
            isSetAsset = true;
        }

        protected internal override UnityEngine.Object GetAsset()
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

        protected internal override UnityEngine.Object GetInstance()
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

        protected internal override UnityObject GetInstance(UnityObject uObj)
        {
            if (uObj != null)
            {
                return UnityObject.Instantiate(uObj);
            }
            return null;
        }

        protected internal override bool IsAlive()
        {
            if (IsNeverDestroy)
            {
                return true;
            }
            return refCount > 0;
        }

        protected internal override bool IsDone()
        {
            return isSetAsset;
        }

        protected internal override void Unload()
        {
            uObject = null;
            isSetAsset = false;
        }
    }
}
#endif