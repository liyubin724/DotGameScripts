using Dot.Map.Lightmap;
using Dot.Map.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Dot.Map
{
    public class MapLightmapData : ScriptableObject,ISerializationCallbackReceiver
    {
        public SceneLightmap[] lightmapDatas = new SceneLightmap[0];

        public StaticObjectData[] staticObjectDatas = new StaticObjectData[0];
        public DynamicObjectData[] dynamicObjectDatas = new DynamicObjectData[0];

        [HideInInspector]
        [NonSerialized]
        public Dictionary<string, string> guidToAssetPathDic = new Dictionary<string, string>();
        [SerializeField]
        private string[] objectGuids = new string[0];
        [SerializeField]
        private string[] objectAssetPaths = new string[0];

        public void OnAfterDeserialize()
        {
            for(int i =0;i<objectGuids.Length;++i)
            {
                guidToAssetPathDic.Add(objectGuids[i], objectAssetPaths[i]);
            }
        }

        public void OnBeforeSerialize()
        {
            objectGuids = guidToAssetPathDic.Keys.ToArray();
            objectAssetPaths = guidToAssetPathDic.Values.ToArray();
        }
    }
}
