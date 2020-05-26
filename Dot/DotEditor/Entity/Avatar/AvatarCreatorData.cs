using Dot.Entity.Avatar;
using Dot.NativeDrawer.Property;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace DotEditor.Entity.Avatar
{
    public class AvatarCreatorData : ScriptableObject
    {
        public SkeletonCreatorData skeletonData = new SkeletonCreatorData();

        public List<PartCreatorData> partDatas = new List<PartCreatorData>();

        [Serializable]
        public class SkeletonCreatorData
        {
            [OpenFilePath(Extension ="fbx")]
            public string fbxPath = string.Empty;
            
            public string savedDir = string.Empty;
        }

        [Serializable]
        public class PartCreatorData
        {
            [EnumButton]
            public AvatarPartType partType = AvatarPartType.Body;
            public List<PrefabCreatorData> prefabParts = new List<PrefabCreatorData>();
            public List<SMRendererCreatorData> rendererParts = new List<SMRendererCreatorData>();
        }

        [Serializable]
        public class PrefabCreatorData
        {
            public string bindNodeName = string.Empty;
            public GameObject bindPrefab = null;
        }

        [Serializable]
        public class SMRendererCreatorData
        {
            [OpenFilePath(Extension = "fbx")]
            public string fbxPath = string.Empty;
        }
    }
}
