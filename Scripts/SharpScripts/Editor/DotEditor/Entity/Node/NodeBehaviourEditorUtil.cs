using Dot.Entity.Node;
using Dot.Util;
using System.Collections.Generic;
using UnityEngine;

namespace DotEditor.Entity.Node
{
    public static class NodeBehaviourEditorUtil
    {
        public static void CopyBindNodeFrom(NodeBehaviour from,NodeBehaviour to)
        {
            if(from == null || to ==null || from.bindNodes == null || from.bindNodes.Length == 0)
            {
                return;
            }
            NodeData[] fromBindNodes = from.bindNodes;
            List<NodeData> toBindNodes = new List<NodeData>();
            Transform toTran = to.gameObject.transform;
            foreach(var node in fromBindNodes)
            {
                if(node == null||node.transform == null)
                {
                    continue;
                }
                string tranName = node.transform.name;
                Transform targetTran = toTran.GetChildByName(tranName);
                if(targetTran!=null)
                {
                    NodeData data = new NodeData();
                    data.name = node.name;
                    data.nodeType = NodeType.BindNode;
                    data.transform = targetTran;
                    toBindNodes.Add(data);
                }
            }

            to.bindNodes = toBindNodes.ToArray();
        }

        public static void AutoFindBoneNode(NodeBehaviour nodeBehaviour)
        {
            nodeBehaviour.boneNodes = new NodeData[0];

            GameObject go = nodeBehaviour.gameObject;
            Transform[] trans = go.GetComponentsInChildren<Transform>(true);
            if(trans!=null && trans.Length>0)
            {
                nodeBehaviour.boneNodes = new NodeData[trans.Length];
                for(int i =0;i<trans.Length;++i)
                {
                    NodeData nodeData = new NodeData();
                    nodeData.nodeType = NodeType.BoneNode;
                    nodeData.name = trans[i].name;
                    nodeData.transform = trans[i];

                    nodeBehaviour.boneNodes[i] = nodeData;
                }
            }
        }

        public static void AutoFindRendererNode(NodeBehaviour nodeBehaviour)
        {
            nodeBehaviour.smRendererNodes = new NodeData[0];

            GameObject go = nodeBehaviour.gameObject;
            SkinnedMeshRenderer[] renderers = go.GetComponentsInChildren<SkinnedMeshRenderer>(true);
            if (renderers != null && renderers.Length > 0)
            {
                nodeBehaviour.smRendererNodes = new NodeData[renderers.Length];
                for (int i = 0; i < renderers.Length; ++i)
                {
                    NodeData nodeData = new NodeData();
                     nodeData.nodeType = NodeType.SMRendererNode;
                    nodeData.name = renderers[i].name;
                    nodeData.renderer = renderers[i];

                    nodeBehaviour.smRendererNodes[i] = nodeData;
                }
            }
        }
    }
}
