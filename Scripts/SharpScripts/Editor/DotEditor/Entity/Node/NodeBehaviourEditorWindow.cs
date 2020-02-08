using Dot.Entity.Node;
using DotEditor.Util;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace DotEditor.Entity.Node
{
    public class NodeBehaviourEditorWindow : EditorWindow
    {
        internal static void ShowWin(NodeBehaviour nodeBehaviour)
        {
            NodeBehaviourEditorWindow win = GetWindow<NodeBehaviourEditorWindow>();
            win.titleContent = new GUIContent("Node Editor");
            win.SetNodeBehaviour(nodeBehaviour);
            win.Show();
        }
        private NodeBehaviour nodeBehaviour = null;

        private ReorderableList bindNodeRList = null;
        private ReorderableList boneNodeRList = null;
        private ReorderableList smRendererNodeRList = null;

        List<NodeData> bindNodeList = null;
        List<NodeData> boneNodeList = null;
        List<NodeData> smRendererNodeList = null;
        internal void SetNodeBehaviour(NodeBehaviour nodeBehaviour)
        {
            this.nodeBehaviour = nodeBehaviour;

            bindNodeList = new List<NodeData>(nodeBehaviour.bindNodes);
            boneNodeList = new List<NodeData>(nodeBehaviour.boneNodes);
            smRendererNodeList = new List<NodeData>(nodeBehaviour.smRendererNodes);

            bindNodeRList = new ReorderableList(bindNodeList, typeof(NodeData), true, true, true, true);
            bindNodeRList.elementHeight = EditorGUIUtility.singleLineHeight * 8;
            bindNodeRList.drawHeaderCallback = (rect) =>
            {
                EditorGUI.LabelField(rect, Contents.BindNodeTitle);
            };
            bindNodeRList.drawElementCallback = (rect, index, isActive, isFocused) =>
            {
                DrawNodeData(rect,index, nodeBehaviour.bindNodes[index]);
            };
            bindNodeRList.onAddCallback = (list) =>
            {
                NodeData data = new NodeData();
                data.nodeType = NodeType.BindNode;
                list.list.Add(data);
            };

            boneNodeRList = new ReorderableList(boneNodeList, typeof(NodeData), true, true, true, true);
            boneNodeRList.elementHeight = EditorGUIUtility.singleLineHeight * 4;
            boneNodeRList.drawHeaderCallback = (rect) =>
            {
                EditorGUI.LabelField(rect, Contents.BoneNodeTitle);
                Rect btnRect = rect;
                btnRect.x = btnRect.x + btnRect.width - 40;
                btnRect.width = 20;
                if(GUI.Button(btnRect,Contents.AutoFindNodeBtnContent))
                {
                    NodeBehaviourEditorUtil.AutoFindBoneNode(nodeBehaviour);
                    boneNodeList.Clear();
                    boneNodeList.AddRange(nodeBehaviour.boneNodes);
                }
                btnRect.x += btnRect.width;
                if(GUI.Button(btnRect,Contents.ClearNodeBtnContent))
                {
                    nodeBehaviour.boneNodes = new NodeData[0];
                    boneNodeList.Clear();
                }
            };
            boneNodeRList.drawElementCallback = (rect, index, isActive, isFocused) =>
            {
                DrawNodeData(rect, index, nodeBehaviour.boneNodes[index]);
            };
            boneNodeRList.onAddCallback = (list) =>
            {
                NodeData data = new NodeData();
                data.nodeType = NodeType.BoneNode;
                list.list.Add(data);
            };

            smRendererNodeRList = new ReorderableList(smRendererNodeList, typeof(NodeData), true, true, true, true);
            smRendererNodeRList.elementHeight = EditorGUIUtility.singleLineHeight * 4;
            smRendererNodeRList.drawHeaderCallback = (rect) =>
            {
                EditorGUI.LabelField(rect, Contents.RendererNodeTitle);
                Rect btnRect = rect;
                btnRect.x = btnRect.x + btnRect.width - 40;
                btnRect.width = 20;
                if (GUI.Button(btnRect, Contents.AutoFindNodeBtnContent))
                {
                    NodeBehaviourEditorUtil.AutoFindRendererNode(nodeBehaviour);
                    smRendererNodeList.Clear();
                    smRendererNodeList.AddRange(nodeBehaviour.smRendererNodes);
                }
                btnRect.x += btnRect.width;
                if (GUI.Button(btnRect, Contents.ClearNodeBtnContent))
                {
                    nodeBehaviour.smRendererNodes = new NodeData[0];
                    smRendererNodeList.Clear();
                }
            };
            smRendererNodeRList.drawElementCallback = (rect, index, isActive, isFocused) =>
            {
                DrawNodeData(rect, index, nodeBehaviour.smRendererNodes[index]);
            };
            smRendererNodeRList.onAddCallback = (list) =>
            {
                NodeData data = new NodeData();
                data.nodeType = NodeType.SMRendererNode;
                list.list.Add(data);
            };

            Repaint();
        }

        private void OnDestroy()
        {
        }

        private int nodeDeleteIndex = -1;
        private void DrawNodeData(Rect rect,int index,NodeData nodeData)
        {
            EditorGUI.LabelField(rect, GUIContent.none, EditorStyles.helpBox);

            Rect drawRect = rect;
            drawRect.x += 2;
            drawRect.y += 2;
            drawRect.width -= 40+8;
            drawRect.height = EditorGUIUtility.singleLineHeight;

            Rect btnRect = rect;
            btnRect.x += btnRect.width - 40-4;
            btnRect.y += 2;
            btnRect.width = 40;
            btnRect.height -= 8;
            if(GUI.Button(btnRect,Contents.DeleteNodeBtnContent))
            {
                nodeDeleteIndex = index;
            }

            NodeType nodeType = nodeData.nodeType;
            EditorGUI.BeginDisabledGroup(true);
            {
                EditorGUI.EnumPopup(drawRect, Contents.NodeTypeContent,nodeType);
            }
            EditorGUI.EndDisabledGroup();

            drawRect.y += drawRect.height;
            nodeData.name = EditorGUI.TextField(drawRect, Contents.NameContent, nodeData.name);

            if (nodeType == NodeType.BindNode || nodeType == NodeType.BoneNode)
            {
                drawRect.y += drawRect.height;
                nodeData.transform = (Transform)EditorGUI.ObjectField(drawRect, Contents.TransformContent, nodeData.transform, typeof(Transform), true);
                if (nodeType == NodeType.BindNode)
                {
                    drawRect.y += drawRect.height;
                    drawRect.height *= 2;
                    nodeData.positionOffset = EditorGUI.Vector3Field(drawRect, Contents.PositionOffsetContent, nodeData.positionOffset);
                    drawRect.y += drawRect.height;
                    nodeData.rotationOffset = EditorGUI.Vector3Field(drawRect, Contents.RotationOffsetContent, nodeData.rotationOffset);
                }
            }
            else if (nodeType == NodeType.SMRendererNode)
            {
                drawRect.y += drawRect.height;
                nodeData.renderer = (SkinnedMeshRenderer)EditorGUI.ObjectField(drawRect, Contents.RendererContent, nodeData.renderer, typeof(SkinnedMeshRenderer), true);
            }
        }

        private Vector2 scrollPos = Vector2.zero;
        private int toolbarSelectIndex = 0;

        private void OnGUI()
        {
            if(nodeBehaviour == null)
            {
                return;
            }
            EditorGUILayout.BeginVertical(EditorStyles.helpBox,GUILayout.ExpandWidth(true),GUILayout.ExpandHeight(true));
            {
                EditorGUILayout.ObjectField(Contents.NodeBehaviourContent, nodeBehaviour, typeof(NodeBehaviour), true);

                int selectedIndex = GUILayout.Toolbar(toolbarSelectIndex, Contents.ToolbarTitle, GUILayout.ExpandWidth(true), GUILayout.Height(40));
                if (selectedIndex != toolbarSelectIndex)
                {
                    nodeDeleteIndex = -1;
                    toolbarSelectIndex = selectedIndex;
                    scrollPos = Vector2.zero;
                }
                scrollPos = EditorGUILayout.BeginScrollView(scrollPos, EditorStyles.helpBox, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
                {
                    if (toolbarSelectIndex == 0)
                    {
                        if(nodeDeleteIndex>=0)
                        {
                            bindNodeRList.list.RemoveAt(nodeDeleteIndex);
                            nodeDeleteIndex = -1;
                        }
                        DrawBindNodes();
                    }
                    else if (toolbarSelectIndex == 1)
                    {
                        if (nodeDeleteIndex >= 0)
                        {
                            boneNodeRList.list.RemoveAt(nodeDeleteIndex);
                            nodeDeleteIndex = -1;
                        }
                        DrawBoneNodes();
                    }
                    else if (toolbarSelectIndex == 2)
                    {
                        if (nodeDeleteIndex >= 0)
                        {
                            smRendererNodeRList.list.RemoveAt(nodeDeleteIndex);
                            nodeDeleteIndex = -1;
                        }
                        DrawRendererNodes();
                    }
                }
                EditorGUILayout.EndScrollView();
            }
            EditorGUILayout.EndVertical();

            

            if(GUI.changed)
            {
                nodeBehaviour.bindNodes = bindNodeList.ToArray();
                nodeBehaviour.boneNodes = boneNodeList.ToArray();
                nodeBehaviour.smRendererNodes = smRendererNodeList.ToArray();
                EditorUtility.SetDirty(nodeBehaviour);
            }
        }

        private void DrawBindNodes()
        {
            if (nodeDeleteIndex >= 0)
            {
                List<NodeData> list = new List<NodeData>(nodeBehaviour.bindNodes);
                list.RemoveAt(nodeDeleteIndex);
                nodeBehaviour.bindNodes = list.ToArray();
                nodeDeleteIndex = -1;

                Repaint();
            }

            bindNodeRList.DoLayoutList();
        }

        private void DrawBoneNodes()
        {
            if (nodeDeleteIndex >= 0)
            {
                List<NodeData> list = new List<NodeData>(nodeBehaviour.bindNodes);
                list.RemoveAt(nodeDeleteIndex);
                nodeBehaviour.boneNodes = list.ToArray();
                nodeDeleteIndex = -1;

                Repaint();
            }

            boneNodeRList.DoLayoutList();
        }

        private void DrawRendererNodes()
        {
            if (nodeDeleteIndex >= 0)
            {
                List<NodeData> list = new List<NodeData>(nodeBehaviour.bindNodes);
                list.RemoveAt(nodeDeleteIndex);
                nodeBehaviour.smRendererNodes = list.ToArray();
                nodeDeleteIndex = -1;

                Repaint();
            }

            smRendererNodeRList.DoLayoutList();
        }


        static class Styles
        {
            internal static GUIStyle bigBoldCenterLableStyle;

            static Styles()
            {
                bigBoldCenterLableStyle = new GUIStyle(EditorStyles.boldLabel);
                bigBoldCenterLableStyle.alignment = TextAnchor.MiddleCenter;
                bigBoldCenterLableStyle.fontSize = 20;
            }
        }

        static class Contents
        {
            internal static GUIContent NodeBehaviourContent = new GUIContent("Node Behaviour");
            internal static GUIContent[] ToolbarTitle = new GUIContent[]
            {
                new GUIContent("Bind Node"),
                new GUIContent("Bone Node"),
                new GUIContent("Renderer Node"),
            };

            internal static GUIContent NodeTypeContent = new GUIContent("Node Type");
            internal static GUIContent NameContent = new GUIContent("Name");
            internal static GUIContent TransformContent = new GUIContent("Transform");
            internal static GUIContent RendererContent = new GUIContent("Renderer");
            internal static GUIContent PositionOffsetContent = new GUIContent("Position Offset");
            internal static GUIContent RotationOffsetContent = new GUIContent("Rotation Offset");

            internal static GUIContent BindNodeTitle = new GUIContent("Bind Nodes");
            internal static GUIContent BoneNodeTitle = new GUIContent("Bone Nodes");
            internal static GUIContent RendererNodeTitle = new GUIContent("Renderer Nodes");

            internal static GUIContent AutoFindNodeBtnContent = new GUIContent("A", "Auto find node");
            internal static GUIContent ClearNodeBtnContent = new GUIContent("C", "Clear all nodes");
            internal static GUIContent DeleteNodeBtnContent = new GUIContent("D", "Delete current node");
        }
    }
}
