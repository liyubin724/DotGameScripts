using UnityEngine;

namespace Dot.Util
{
    public class DontDestoryBehaviour : MonoBehaviour
    {
        void Awake()
        {
            DontDestroyHandler.AddTransform(transform);
            Destroy(this);
        }
    }
}
