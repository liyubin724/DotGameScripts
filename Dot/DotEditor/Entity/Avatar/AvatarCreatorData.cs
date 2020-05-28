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
        public PartOutputData partOutputData = new PartOutputData();

        public override string ToString()
        {
            return name;
        }

        [Serializable]
        public class SkeletonCreatorData
        {
            [OpenFolderPath]
            public string outputFolder = string.Empty;
            public GameObject fbx;

            public string GetTargetPrefabPath()
            {
                if(string.IsNullOrEmpty(outputFolder) || fbx == null)
                {
                    return null;
                }
                return $"{outputFolder}/{fbx.name}_skeleton.prefab";
            }
        }

        [Serializable]
        public class PartOutputData
        {
            [OpenFolderPath]
            public string outputFolder = string.Empty;
            public List<PartCreatorData> partDatas = new List<PartCreatorData>();
        }

        [Serializable]
        public class PartCreatorData
        {
            public string name = "";
            [EnumButton]
            public AvatarPartType partType = AvatarPartType.Body;

            public List<SMRendererCreatorData> smRendererDatas = new List<SMRendererCreatorData>();
            public List<PrefabCreatorData> prefabDatas = new List<PrefabCreatorData>();

            public string GetTargetPath(string outputDir)
            {
                return $"{outputDir}/{name}_part.asset";
            }
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
            public GameObject partFbx;
            public bool IsCopyMesh = true;
        }
    }
}
