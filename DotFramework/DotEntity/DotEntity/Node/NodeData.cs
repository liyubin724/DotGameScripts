using System;
using UnityEngine;

namespace Dot.Entity.Node
{
    [Serializable]
    public class NodeData
    {
        public NodeType nodeType = NodeType.None;
        public string name = string.Empty;
        public Transform transform = null;
        public SkinnedMeshRenderer renderer = null;
    }
}
