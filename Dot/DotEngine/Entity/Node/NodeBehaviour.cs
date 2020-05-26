using System;
using System.Collections.Generic;
using UnityEngine;

namespace Dot.Entity.Node
{
    public enum NodeType
    {
        BindNode,
        SMRendererNode,
        BoneNode,
    }

    [Serializable]
    public class NodeData
    {
        public NodeType nodeType = NodeType.BindNode;
        public string name = string.Empty;
        public Transform transform = null;
        public SkinnedMeshRenderer renderer = null;
    }

    public class NodeBehaviour : MonoBehaviour
    {
        public NodeData[] bindNodes = new NodeData[0];
        public NodeData[] boneNodes = new NodeData[0];
        public NodeData[] smRendererNodes = new NodeData[0];

        private Dictionary<NodeType, Dictionary<string, NodeData>> nodeDic = new Dictionary<NodeType, Dictionary<string, NodeData>>();

        private void Awake()
        {
            Dictionary<string, NodeData> dataDic = new Dictionary<string, NodeData>();
            nodeDic.Add(NodeType.BindNode, dataDic);
            foreach (var data in bindNodes)
            {
                dataDic.Add(data.name, data);
            }

            dataDic = new Dictionary<string, NodeData>();
            nodeDic.Add(NodeType.BoneNode, dataDic);
            foreach (var data in boneNodes)
            {
                dataDic.Add(data.name, data);
            }
            dataDic = new Dictionary<string, NodeData>();
            nodeDic.Add(NodeType.SMRendererNode, dataDic);
            foreach (var data in smRendererNodes)
            {
                dataDic.Add(data.name, data);
            }
        }

        public NodeData GetNode(NodeType nodeType, string name)
        {
            if (nodeDic.TryGetValue(nodeType, out Dictionary<string, NodeData> dataDic))
            {
                if (dataDic.TryGetValue(name, out NodeData data))
                {
                    return data;
                }
            }
            return null;
        }

        public NodeData GetBindNode(string name)
        {
            return GetNode(NodeType.BindNode, name);
        }

        public NodeData GetBoneNode(string name)
        {
            return GetNode(NodeType.BoneNode, name);
        }

        public NodeData GetSMRendererNode(string name)
        {
            return GetNode(NodeType.SMRendererNode, name);
        }

        public Transform[] GetBoneTransformByNames(string[] names)
        {
            if (names == null || names.Length == 0)
                return new Transform[0];

            Transform[] transforms = new Transform[names.Length];
            for (int i = 0; i < names.Length; i++)
            {
                transforms[i] = GetNode(NodeType.BoneNode, names[i])?.transform;
            }
            return transforms;
        }
    }
}
