using Dot.Map.Lightmap;
using System;

namespace Dot.Map.Item
{
    [Serializable]
    public class StaticItemData
    {
        public string guid;
        public string assetPath;

        public ItemPlacementData placementData;
        public RendererLightmapData[] rendererDatas = new RendererLightmapData[0];
    }
}
