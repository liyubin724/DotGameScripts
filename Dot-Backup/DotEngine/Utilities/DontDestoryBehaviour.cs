using UnityEngine;

namespace Dot.Utilities
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