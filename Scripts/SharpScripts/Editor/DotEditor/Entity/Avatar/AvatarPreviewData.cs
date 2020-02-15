using Dot.Entity.Avatar;
using System.Collections.Generic;
using UnityEngine;

namespace DotEditor.Entity.Avatar
{
    public class AvatarPreviewData : ScriptableObject
    {
        public string dataName = string.Empty;
        public List<GameObject> skeletonPefabs = new List<GameObject>();
        public List<AvatarPartData> partDatas = new List<AvatarPartData>();
    }
}
