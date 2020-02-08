using UnityEngine;

namespace Dot.Entity.Avatar
{
    public class AvatarRendererPartData : ScriptableObject
    {
        public string rendererNodeName = "";

        public string rootBoneName = "";
        public Mesh mesh = null;

        public Material[] materials = new Material[0];
        public string[] boneNames = new string[0];
    }
}
