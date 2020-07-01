using UnityEngine;

namespace Dot.Entity.Avatar
{
    public enum AvatarPartType
    {
        Feet = 0,
        Body,
        Hand,
        Head,
        Weapon,
    }

    public class AvatarPartInstance
    {
        public AvatarPartType partType = AvatarPartType.Feet;
        public SkinnedMeshRenderer[] renderers = new SkinnedMeshRenderer[0];
        public GameObject[] gameObjects = new GameObject[0];
    }
}
