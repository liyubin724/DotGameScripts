using Dot.Map.Item;
using UnityEngine;

namespace Dot.Map
{
    public class MapBehaviour : MonoBehaviour
    {
        private static MapBehaviour instance = null;
        public static MapBehaviour Instance
        {
            get
            {
                return instance;
            }
        }

        private MapLightmapData lightmapData = null;
        private ItemBehaviour[] existItems = new ItemBehaviour[0];
        private int lightmapStartIndex = 0;

        public void DoInit()
        {
            InitLightMap();
        }

        public void DoDestroy()
        {

        }

        private void InitLightMap()
        {
            if(lightmapData!=null && lightmapData.lightmapDatas!=null && lightmapData.lightmapDatas.Length>0)
            {
                var lightmaps = LightmapSettings.lightmaps;
                var targetLightmaps = new LightmapData[lightmaps.Length + lightmapData.lightmapDatas.Length];
                lightmaps.CopyTo(targetLightmaps, 0);
                lightmapStartIndex = lightmaps.Length;
                foreach(var data in lightmapData.lightmapDatas)
                for(int i=lightmapStartIndex;i<targetLightmaps.Length;++i)
                {
                    var lightmap = new LightmapData()
                    {
                        lightmapColor = data.lightmapColor,
                        lightmapDir = data.lightmapDir,
                        shadowMask = data.shadowMask,
                    };
                        targetLightmaps[i] = lightmap;
                }

                LightmapSettings.lightmaps = targetLightmaps;
            }
        }

        private void InitExistItem()
        {
            if(existItems !=null && existItems.Length>0)
            {

            }
        }

        void Awake()
        {
            instance = this;
        }

        void OnDestroy()
        {
            instance = null;
        }
    }
}
