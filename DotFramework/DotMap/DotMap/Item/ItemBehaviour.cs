using UnityEngine;

namespace Dot.Map.Item
{
    public class ItemBehaviour : MonoBehaviour
    {
        public string guid = null;
        public Renderer[] renderers = new Renderer[0];
        public GameObject cachedGameObject = null;
        public Transform cachedTransform = null;

        void Awake()
        {
            if(cachedGameObject == null)
            {
                cachedGameObject = gameObject;
            }
            if(cachedTransform == null)
            {
                cachedTransform = transform;
            }
        }

        public Renderer GetRenderer(int index)
        {
            if (index >= 0&&index<renderers.Length)
            {
                return renderers[index];
            }
            return null;
        }
    }
}
