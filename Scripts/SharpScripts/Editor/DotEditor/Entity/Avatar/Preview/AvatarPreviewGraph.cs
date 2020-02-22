using Dot.Entity.Avatar;
using Dot.XNodeEx;
using System.Collections.Generic;
using UnityEngine;

namespace DotEditor.Entity.Avatar.Preview
{
    [CreateAssetMenu(fileName = "preview_graph", menuName = "Entity/Avatar/Preview Graph")]
    public class AvatarPreviewGraph : DotNodeGraph
    {
        public List<GameObject> skeletonList = new List<GameObject>();
        public List<AvatarPartData> partList = new List<AvatarPartData>();

        public GameObject[] GetSkeletons()
        {
            return skeletonList.ToArray();
        }

        public AvatarPartData[] GetParts(AvatarPartType partType)
        {
            List<AvatarPartData> parts = new List<AvatarPartData>();
            foreach(var part in partList)
            {
                if(part!=null && part.partType == partType)
                {
                    parts.Add(part);
                }
            }
            return parts.ToArray();
        }
    }
}
