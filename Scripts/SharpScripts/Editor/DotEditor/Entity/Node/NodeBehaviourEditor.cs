using Dot.Entity.Node;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace DotEditor.Entity.Node
{
    [CustomEditor(typeof(NodeBehaviour))]
    public class NodeBehaviourEditor : Editor
    {
        private bool isBindNodeVisible = false;
        private bool isBoneNodeVisible = false;
        private bool isRendererNodeVisible = false;

        private ReorderableList rlBindNodeList = null;
        private ReorderableList rlBoneNodeList = null;
        private ReorderableList rlSMRendererNodeList = null;

        private NodeBehaviour nodeBehaviour = null;
        void OnEnable()
        {
            nodeBehaviour = target as NodeBehaviour;

            rlBindNodeList = new ReorderableList(nodeBehaviour.bindNodes, typeof(NodeData), false, true, false, false);
            rlBindNodeList.elementHeight = EditorGUIUtility.singleLineHeight * 5;
            rlBindNodeList.drawHeaderCallback = (rect) =>
            {
                EditorGUI.LabelField(rect, "Bind Nodes");
            };
            rlBindNodeList.drawElementCallback = (rect, index, isActive, isFocused) =>
            {
                DrawNodeData(rect, nodeBehaviour.bindNodes[index]);
            };

            rlBoneNodeList = new ReorderableList(nodeBehaviour.boneNodes, typeof(NodeData), false, true, false, false);
            rlBoneNodeList.elementHeight = EditorGUIUtility.singleLineHeight * 3;
            rlBoneNodeList.drawHeaderCallback = (rect) =>
            {
                EditorGUI.LabelField(rect, "Bone Nodes");
            };
            rlBoneNodeList.drawElementCallback = (rect, index, isActive, isFocused) =>
            {
                DrawNodeData(rect, nodeBehaviour.boneNodes[index]);
            };

            rlSMRendererNodeList = new ReorderableList(nodeBehaviour.smRendererNodes, typeof(NodeData), false, true, false, false);
            rlSMRendererNodeList.elementHeight = EditorGUIUtility.singleLineHeight * 3;
            rlSMRendererNodeList.drawHeaderCallback = (rect) =>
            {
                EditorGUI.LabelField(rect, "Renderer Nodes");
            };
            rlSMRendererNodeList.drawElementCallback = (rect, index, isActive, isFocused) =>
            {
                DrawNodeData(rect, nodeBehaviour.smRendererNodes[index]);
            };
        }

        private void DrawNodeData(Rect rect,NodeData nodeData)
        {
            NodeType nodeType = nodeData.nodeType;
            EditorGUI.BeginDisabledGroup(true);
            {
                Rect drawRect = rect;
                drawRect.height = EditorGUIUtility.singleLineHeight;
                EditorGUI.TextField(drawRect, Contents.NameContent, nodeData.name);

                if(nodeType == NodeType.BindNode || nodeType == NodeType.BoneNode)
                {
                    drawRect.y += drawRect.height;
                    EditorGUI.ObjectField(drawRect, Contents.TransformContent, nodeData.transform, typeof(Transform), false);
                }else  if(nodeType == NodeType.SMRendererNode)
                {
                    drawRect.y += drawRect.height;
                    EditorGUI.ObjectField(drawRect, Contents.RendererContent, nodeData.renderer, typeof(SkinnedMeshRenderer), false);
                }
            }
            EditorGUI.EndDisabledGroup();
        }
        
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            isBindNodeVisible = EditorGUILayout.Toggle("Bind Node:", isBindNodeVisible);
            if(isBindNodeVisible)
            {
                rlBindNodeList.DoLayoutList();
            }

            isBoneNodeVisible = EditorGUILayout.Toggle("Bone Node:", isBoneNodeVisible);
            if(isBoneNodeVisible)
            {
                rlBoneNodeList.DoLayoutList();
            }

            isRendererNodeVisible = EditorGUILayout.Toggle("Renderer Node:", isRendererNodeVisible);
            if(isRendererNodeVisible)
            {
                rlSMRendererNodeList.DoLayoutList();
            }

            serializedObject.ApplyModifiedProperties();

            if(GUILayout.Button("Editor",GUILayout.Height(40)))
            {
                NodeBehaviourEditorWindow.ShowWin(target as NodeBehaviour);
            }
        }

        class Contents
        {
            internal static GUIContent NameContent = new GUIContent("Name");
            internal static GUIContent TransformContent = new GUIContent("Transform");
            internal static GUIContent RendererContent = new GUIContent("Renderer");
            internal static GUIContent PositionOffsetContent = new GUIContent("Position Offset");
            internal static GUIContent RotationOffsetContent = new GUIContent("Rotation Offset");
        }
    }
}
