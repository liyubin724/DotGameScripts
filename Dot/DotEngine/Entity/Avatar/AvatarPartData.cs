using UnityEngine;

namespace Dot.Entity.Avatar
{
    public class AvatarPartData : ScriptableObject
    {
        public AvatarPartType partType = AvatarPartType.None;

        public AvatarRendererPartData[] rendererParts = new AvatarRendererPartData[0];
        public AvatarPrefabPartData[] prefabParts = new AvatarPrefabPartData[0];
    }
}
