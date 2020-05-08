using UnityObject = UnityEngine.Object;

namespace Dot.Core.Extension
{
    public static class UnityObjectExtensions
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
