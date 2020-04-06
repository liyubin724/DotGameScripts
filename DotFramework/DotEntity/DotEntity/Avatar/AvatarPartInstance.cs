using UnityEngine;

namespace Dot.Entity.Avatar
{
    public enum AvatarPartType
    {
        None = 0,

        Feet,
        Body,
        Hand,
        Head,
        Weapon,

        Max,
    }

    public class AvatarPartInstance
    {
        public AvatarPartType partType = AvatarPartType.None;
        public SkinnedMeshRenderer[] renderers = new SkinnedMeshRenderer[0];
        public GameObject[] gameObjects = new GameObject[0];
    }
}
