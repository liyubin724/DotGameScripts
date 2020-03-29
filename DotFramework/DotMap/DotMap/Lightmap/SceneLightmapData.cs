using System;
using UnityEngine;

namespace Dot.Map.Lightmap
{
    [Serializable]
    public class SceneLightmapData
    {
        public Texture2D lightmapColor;
        public Texture2D lightmapDir;
        public Texture2D shadowMask;
    }
}
