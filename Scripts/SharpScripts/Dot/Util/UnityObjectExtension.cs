using UnityEngine;
using UnityObject = UnityEngine.Object;

namespace Dot.Util
{
    public static class UnityObjectExtension
    {
        public static bool IsNull(this UnityObject obj)
        {
            if (obj == null || obj.Equals(null))
            {
                return true;
            }

            return false;
        }

        public static Transform GetChildByName(this Transform tran,string name)
        {
            if(tran.name == name)
            {
                return tran;
            }

            for(int i =0;i<tran.childCount;++i)
            {
                Transform target = GetChildByName(tran.GetChild(i), name);
                if(target!=null)
                {
                    return target;
                }
            }

            return null;
        }
    }
}
