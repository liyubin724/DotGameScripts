using Dot.Entity.Avatar;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace DotEditor.Entity.Avatar
{
    public class AvatarCreatorData : ScriptableObject
    {
        public string dataName = string.Empty;
        public bool isEnable = true;

        public List<AvatarSkeletonCreatorData> skeletonCreatorDatas = new List<AvatarSkeletonCreatorData>();
        public List<AvatarPartCreatorData> partCreatorDatas = new List<AvatarPartCreatorData>();

        [Serializable]
        public class AvatarSkeletonCreatorData
        {
            public string dataName = string.Empty;
            public bool isEnable = true;
            public string savedDir = string.Empty;

            public GameObject fbxPrefab = null;
        }

        [Serializable]
        public class AvatarPartCreatorData
        {
            public string dataName = string.Empty;
            public bool isEnable = true;
            public string savedDir = string.Empty;

            public AvatarPartType partType = AvatarPartType.None;
            public List<AvatarPrefabCreatorData> prefabPartDatas = new List<AvatarPrefabCreatorData>();
            public List<AvatarRendererCreatorData> rendererPartDatas = new List<AvatarRendererCreatorData>();
        }

        [Serializable]
        public class AvatarPrefabCreatorData
        {
            public string dataName = string.Empty;
            public bool isEnable = true;
            public string savedDir = string.Empty;

            public string bindNodeName = string.Empty;
            public GameObject bindPrefab = null;
        }

        [Serializable]
        public class AvatarRendererCreatorData
        {
            public string dataName = string.Empty;
            public bool isEnable = true;
            public string savedDir = string.Empty;

            public GameObject fbxPrefab = null;
        }
    }
}
