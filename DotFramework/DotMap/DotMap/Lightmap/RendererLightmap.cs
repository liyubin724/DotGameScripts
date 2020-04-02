using System;
using UnityEngine;

namespace Dot.Map.Lightmap
{
    [Serializable]
    public class RendererLightmap
    {
        public int rendererIndex;
        public int lightmapIndex;
        public Vector4 lightmapOffsetScale;
    }
}
