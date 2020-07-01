//using System.Collections.Generic;
//using UnityEngine;

//namespace Dot.Map
//{
//    public class MapRootBehaviour : MonoBehaviour
//    {
//        private static MapRootBehaviour instance = null;
//        public static MapRootBehaviour Instance
//        {
//            get
//            {
//                return instance;
//            }
//        }

//        public Transform[] rootChilds = new Transform[0];
//        public MapLightmapData lightmapData = null;

//        private Dictionary<string, Transform> rootChildDic = new Dictionary<string, Transform>();
//        void Awake()
//        {
//            foreach(var child in rootChilds)
//            {
//                rootChildDic.Add(child.name, child);
//            }

//            instance = this;
//        }

//        private void InitSceneLightmap()
//        {
//            if (lightmapData != null)
//            {
//                LightmapData[] lightmaps = new LightmapData[lightmapData.lightmapDatas.Length];
//                for (int i = 0; i < lightmapData.lightmapDatas.Length; ++i)
//                {
//                    LightmapData lightmap = new LightmapData()
//                    {
//                        lightmapColor = lightmapData.lightmapDatas[i].lightmapColor,
//                        lightmapDir = lightmapData.lightmapDatas[i].lightmapDir,
//                        shadowMask = lightmapData.lightmapDatas[i].shadowMask,
//                    };
//                    lightmaps[i] = lightmap;
//                }
//                LightmapSettings.lightmaps = lightmaps;
//            }
//        }


//        void OnDestroy()
//        {
//            instance = null;
//        }
//    }
//}
