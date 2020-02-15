using UnityEngine;

namespace Dot.Entity.Avatar
{
    public enum AvatarPartType
    {
        None = 0,

        Jiao,
        Shen,
        Shou,
        Tou,
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
