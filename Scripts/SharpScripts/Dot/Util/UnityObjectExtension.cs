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
    }
}
