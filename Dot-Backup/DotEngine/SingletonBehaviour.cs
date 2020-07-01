using Dot.Utilities;
using UnityEngine;
using UnityObject = UnityEngine.Object;

namespace Dot
{
    public abstract class SingletonBehaviour<T> : MonoBehaviour where T:SingletonBehaviour<T>
    {
        private static T instance = null;
        public static T GetInstance()
        {
            if (instance == null)
            {
                instance = UnityObject.FindObjectOfType<T>();
                if(instance == null)
                {
                    instance = DontDestroyHandler.CreateComponent<T>();
                }else
                {
                    DontDestroyHandler.AddTransform(instance.transform);
                }
            }
            return instance;
        }

        protected virtual void OnDestroy()
        {
            instance = null;
        }
    }
}