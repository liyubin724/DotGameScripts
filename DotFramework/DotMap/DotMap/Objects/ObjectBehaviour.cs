using Dot.Log;
using Dot.Map.Lightmap;
using UnityEngine;

namespace Dot.Map.Objects
{
    public class ObjectBehaviour : MonoBehaviour
    {
        public string guid = null;
        
        private Transform cachedTransform = null;
        public Transform CachedTransform
        {
            get
            {
                if(cachedTransform == null)
                {
                    cachedTransform = transform;
                }
                return cachedTransform;
            }
        }

        private GameObject cachedGameObject = null;
        public GameObject CachedGameObject
        {
            get
            {
                if(cachedGameObject == null)
                {
                    cachedGameObject = gameObject;
                }
                return cachedGameObject;
            }
        }

        public Renderer[] renderers = new Renderer[0];
        public void SetPlacement(Transform parentTran,ObjectPlacementData placementData)
        {
            if(parentTran!=null)
            {
                CachedTransform.SetParent(parentTran, false);
            }
            CachedTransform.localPosition = placementData.localPosition;
            CachedTransform.localRotation = Quaternion.Euler(placementData.localRotation);
            CachedTransform.localScale = placementData.localScale;
        }

        public void SetLightmap(RendererLightmap[] lightmaps)
        {
            if(lightmaps.Length != renderers.Length)
            {
                LogUtil.LogWarning(this.GetType(), "SetLightmap::the lenght of renderers is not equal to the length of lightmap");
                return;
            }

            for(int i =0;i<lightmaps.Length;++i)
            {
                RendererLightmap lightmap = lightmaps[i];
                Renderer renderer = renderers[lightmap.rendererIndex];
                renderer.lightmapIndex = lightmap.rendererIndex;
                renderer.lightmapScaleOffset = lightmap.lightmapOffsetScale;
            }
        }
    }
}
