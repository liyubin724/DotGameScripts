using Dot.NativeDrawer.Property;
using System;
using UnityEngine;

namespace Dot.Entity.Avatar
{
    public class AvatarPartData : ScriptableObject
    {
        [EnumButton]
        public AvatarPartType partType = AvatarPartType.Feet;
        [Readonly]
        public AvatarRendererPartData[] rendererParts = new AvatarRendererPartData[0];
        [Readonly]
        public AvatarPrefabPartData[] prefabParts = new AvatarPrefabPartData[0];

        [Serializable]
        public class AvatarRendererPartData
        {
            public string rendererNodeName = "";

            public string rootBoneName = "";
            public string[] boneNames = new string[0];

            public Mesh mesh = null;
            public Material[] materials = new Material[0];
        }

        [Serializable]
        public class AvatarPrefabPartData
        {
            public string bindNodeName = "";
            public GameObject prefabGO = null;
        }
    }
}
