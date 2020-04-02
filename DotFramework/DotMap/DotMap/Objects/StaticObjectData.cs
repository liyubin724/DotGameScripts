using Dot.Map.Lightmap;
using System;

namespace Dot.Map.Objects
{
    [Serializable]
    public class StaticObjectData
    {
        public string guid;
        public ObjectPlacementData placementData;
        public RendererLightmap[] rendererDatas = new RendererLightmap[0];
    }
}
