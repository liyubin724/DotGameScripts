using System.Collections.Generic;
using UnityEngine;
using System;

namespace Dot.Entity.Node
{
    public enum NodeType
    {
        None = 0,
        BindNode,
        SMRendererNode,
        BoneNode,
    }

    [Serializable]
    public class NodeData
    {
        public NodeType nodeType = NodeType.None;
        public string name = string.Empty;
        public Transform transform = null;
        public SkinnedMeshRenderer renderer = null;
        public Vector3 positionOffset = Vector3.zero;
        public Vector3 rotationOffset = Vector3.zero;
    }
    
    public class NodeBehaviour : MonoBehaviour
    {
        [SerializeField]
        private NodeData[] bindNodes = new NodeData[0];
        [SerializeField]
        private NodeData[] boneNodes = new NodeData[0];
        [SerializeField]
        private NodeData[] smRendererNodes = new NodeData[0];

        private Dictionary<NodeType, Dictionary<string, NodeData>> nodeDic = new Dictionary<NodeType, Dictionary<string, NodeData>>();

        private void Awake()
        {
            Dictionary<string, NodeData> dataDic = new Dictionary<string, NodeData>();
            nodeDic.Add(NodeType.BindNode, dataDic);
            foreach(var data in bindNodes)
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

        public NodeData GetNode(NodeType nodeType,string name)
        {
            if (nodeDic.TryGetValue(nodeType, out Dictionary<string, NodeData> dataDic))
            {
                if(dataDic.TryGetValue(name,out NodeData data))
                {
                    return data;
                }
            }
            return null;
        }

        public Transform[] GetBoneByNames(string[] names)
        {
            if (names == null || names.Length == 0)
                return new Transform[0];

            Transform[] transforms = new Transform[names.Length];
            for(int i =0;i<names.Length;i++)
            {
                transforms[i] = GetNode(NodeType.BoneNode,names[i])?.transform;
            }
            return transforms;
        }
    }
}
