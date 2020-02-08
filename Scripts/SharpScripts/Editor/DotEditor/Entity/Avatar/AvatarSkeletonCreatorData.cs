using System;
using System.Collections.Generic;
using UnityEngine;

namespace DotEditor.Entity.Avatar
{
    public class AvatarSkeletonCreatorData : ScriptableObject
    {
        public string creatorName = "";
        public string savedAssetDir = "";
        public List<AvatarSkeletonData> datas = new List<AvatarSkeletonData>();

        [Serializable]
        public class AvatarSkeletonData
        {
            public bool isEnable = true;
            public GameObject fbxPrefab = null;
        }
    }
}
