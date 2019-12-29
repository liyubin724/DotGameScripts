#if UNITY_EDITOR
using UnityObject = UnityEngine.Object;

namespace Dot.Asset
{
    public class DatabaseAssetNode : AssetNode
    {
        public UnityObject UObject { get; set; }

        public override UnityEngine.Object GetAsset()
        {
            return UObject;
        }

        public override UnityEngine.Object GetInstance()
        {
            if(UObject!=null)
            {
                var instance = UnityObject.Instantiate(UObject);
                return instance;
            }

            return null;
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
            return UObject != null;
        }
    }
}
#endif