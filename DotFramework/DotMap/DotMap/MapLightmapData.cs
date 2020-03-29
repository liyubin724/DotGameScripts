using Dot.Map.Lightmap;
using Dot.Map.Item;
using System;
using UnityEngine;

namespace Dot.Map
{
    public class MapLightmapData : ScriptableObject
    {
        public SceneLightmapData[] lightmapDatas = new SceneLightmapData[0];
        public DynamicItemData[] dynamicObjectDatas = new DynamicItemData[0];
        public StaticItemData[] staticObjectDatas = new StaticItemData[0];
    }
}
