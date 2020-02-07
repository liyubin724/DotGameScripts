using Dot.Entity.Node;
using System;
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

        private SerializedObject serializedObject = null;
        private SerializedProperty bindNodes;
        private SerializedProperty boneNodes;
        private SerializedProperty smRendererNodes;

        private ReorderableList rlBindNodeList = null;
        private ReorderableList rlBoneNodeList = null;
        private ReorderableList rlSMRendererNodeList = null;

        internal void SetNodeBehaviour(NodeBehaviour nodeBehaviour)
        {
            this.nodeBehaviour = nodeBehaviour;

            serializedObject = new SerializedObject(nodeBehaviour);
            bindNodes = serializedObject.FindProperty("bindNodes");
            boneNodes = serializedObject.FindProperty("boneNodes");
            smRendererNodes = serializedObject.FindProperty("smRendererNodes");

            rlBindNodeList = new ReorderableList(serializedObject, bindNodes, true, true, true, true);
            rlBindNodeList.elementHeight = EditorGUIUtility.singleLineHeight * 8;
            rlBindNodeList.drawHeaderCallback = (rect) =>
            {
                EditorGUI.LabelField(rect, Contents.BindNodeTitle);
            };
            rlBindNodeList.drawElementCallback = (rect, index, isActive, isFocused) =>
            {
                DrawNodeData(rect,index, bindNodes);
            };
            rlBindNodeList.onAddCallback = (list) =>
            {
                bindNodes.InsertArrayElementAtIndex(bindNodes.arraySize);
                SerializedProperty nodeData = bindNodes.GetArrayElementAtIndex(bindNodes.arraySize - 1);
                SerializedProperty nodeType = nodeData.FindPropertyRelative("nodeType");
                string[] names = nodeType.enumNames;
                nodeType.enumValueIndex = Array.IndexOf(names, NodeType.BindNode.ToString());
            };

            rlBoneNodeList = new ReorderableList(serializedObject, boneNodes, true, true, true, true);
            rlBoneNodeList.elementHeight = EditorGUIUtility.singleLineHeight * 4;
            rlBoneNodeList.drawHeaderCallback = (rect) =>
            {
                EditorGUI.LabelField(rect, Contents.BoneNodeTitle);
                Rect btnRect = rect;
                btnRect.x = btnRect.x + btnRect.width - 40;
                btnRect.width = 20;
                if(GUI.Button(btnRect,Contents.AutoFindNodeBtnContent))
                {
                    boneNodes.ClearArray();
                    GameObject go = nodeBehaviour.gameObject;
                    for (int i = 0; i < go.transform.childCount; ++i)
                    {
                        Transform tran = go.transform.GetChild(i);
                        if(tran.GetComponent<Renderer>() == null)
                        {
                            Transform[] childs = go.GetComponentsInChildren<Transform>();
                            foreach(var child in childs)
                            {
                                if(child.GetComponent<Renderer>() == null)
                                {
                                    boneNodes.InsertArrayElementAtIndex(boneNodes.arraySize);
                                    SerializedProperty nodeData = boneNodes.GetArrayElementAtIndex(boneNodes.arraySize - 1);
                                    SerializedProperty nodeType = nodeData.FindPropertyRelative("nodeType");
                                    string[] names = nodeType.enumNames;
                                    nodeType.enumValueIndex = Array.IndexOf(names, NodeType.BoneNode.ToString());
                                    nodeData.FindPropertyRelative("transform").objectReferenceValue = child;
                                    nodeData.FindPropertyRelative("name").stringValue = child.name;
                                }
                            }
                        }
                    }
                }
                btnRect.x += btnRect.width;
                if(GUI.Button(btnRect,Contents.ClearNodeBtnContent))
                {
                    boneNodes.ClearArray();
                }
            };
            rlBoneNodeList.drawElementCallback = (rect, index, isActive, isFocused) =>
            {
                DrawNodeData(rect, index, boneNodes);
            };
            rlBoneNodeList.onAddCallback = (list) =>
            {
                boneNodes.InsertArrayElementAtIndex(boneNodes.arraySize);
                SerializedProperty nodeData = boneNodes.GetArrayElementAtIndex(boneNodes.arraySize - 1);
                SerializedProperty nodeType = nodeData.FindPropertyRelative("nodeType");
                string[] names = nodeType.enumNames;
                nodeType.enumValueIndex = Array.IndexOf(names, NodeType.BoneNode.ToString());
            };

            rlSMRendererNodeList = new ReorderableList(serializedObject, smRendererNodes, true, true, true, true);
            rlSMRendererNodeList.elementHeight = EditorGUIUtility.singleLineHeight * 4;
            rlSMRendererNodeList.drawHeaderCallback = (rect) =>
            {
                EditorGUI.LabelField(rect, Contents.RendererNodeTitle);
                Rect btnRect = rect;
                btnRect.x = btnRect.x + btnRect.width - 40;
                btnRect.width = 20;
                if (GUI.Button(btnRect, Contents.AutoFindNodeBtnContent))
                {
                    smRendererNodes.ClearArray();
                    GameObject go = nodeBehaviour.gameObject;
                    SkinnedMeshRenderer[] renderers = go.GetComponentsInChildren<SkinnedMeshRenderer>(true);
                    if(renderers!=null && renderers.Length>0)
                    {
                        for(int i =0;i<renderers.Length;++i)
                        {
                            smRendererNodes.InsertArrayElementAtIndex(i);
                            SerializedProperty nodeData = smRendererNodes.GetArrayElementAtIndex(i);
                            SerializedProperty nodeType = nodeData.FindPropertyRelative("nodeType");
                            string[] names = nodeType.enumNames;
                            nodeType.enumValueIndex = Array.IndexOf(names, NodeType.SMRendererNode.ToString());
                            nodeData.FindPropertyRelative("renderer").objectReferenceValue = renderers[i];
                            nodeData.FindPropertyRelative("name").stringValue = renderers[i].name;
                        }
                    }
                }
                btnRect.x += btnRect.width;
                if (GUI.Button(btnRect, Contents.ClearNodeBtnContent))
                {
                    smRendererNodes.ClearArray();
                }
            };
            rlSMRendererNodeList.drawElementCallback = (rect, index, isActive, isFocused) =>
            {
                DrawNodeData(rect, index, smRendererNodes);
            };
            rlSMRendererNodeList.onAddCallback = (list) =>
            {
                smRendererNodes.InsertArrayElementAtIndex(smRendererNodes.arraySize);
                SerializedProperty nodeData = smRendererNodes.GetArrayElementAtIndex(smRendererNodes.arraySize - 1);
                SerializedProperty nodeType = nodeData.FindPropertyRelative("nodeType");
                string[] names = nodeType.enumNames;
                nodeType.enumValueIndex = Array.IndexOf(names, NodeType.SMRendererNode.ToString());
            };

            Repaint();
        }

        private int nodeDeleteIndex = -1;
        private void DrawNodeData(Rect rect,int index,SerializedProperty property)
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

            SerializedProperty nodeData = property.GetArrayElementAtIndex(index);

            SerializedProperty nodeTypeProperty = nodeData.FindPropertyRelative("nodeType");
            EditorGUI.BeginDisabledGroup(true);
            {
                EditorGUI.PropertyField(drawRect, nodeTypeProperty);
            }
            EditorGUI.EndDisabledGroup();

            drawRect.y += drawRect.height;
            EditorGUI.PropertyField(drawRect, nodeData.FindPropertyRelative("name"));

            NodeType nodeType = (NodeType)nodeTypeProperty.intValue;
            if (nodeType == NodeType.BindNode || nodeType == NodeType.BoneNode)
            {
                drawRect.y += drawRect.height;
                EditorGUI.PropertyField(drawRect, nodeData.FindPropertyRelative("transform"));
                if (nodeType == NodeType.BindNode)
                {
                    drawRect.y += drawRect.height;
                    drawRect.height *= 2;
                    EditorGUI.PropertyField(drawRect, nodeData.FindPropertyRelative("positionOffset"));
                    drawRect.y += drawRect.height;
                    EditorGUI.PropertyField(drawRect, nodeData.FindPropertyRelative("rotationOffset"));
                }
            }
            else if (nodeType == NodeType.SMRendererNode)
            {
                drawRect.y += drawRect.height;
                EditorGUI.PropertyField(drawRect, nodeData.FindPropertyRelative("renderer"));
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
                EditorGUILayout.ObjectField(Contents.NodeBehaviourContent, nodeBehaviour, typeof(NodeBehaviour), false);

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
                        DrawBindNodes();
                    }
                    else if (toolbarSelectIndex == 1)
                    {
                        DrawBoneNodes();
                    }
                    else if (toolbarSelectIndex == 2)
                    {
                        DrawRendererNodes();
                    }
                }
                EditorGUILayout.EndScrollView();
            }
            EditorGUILayout.EndVertical();
        }

        private void DrawBindNodes()
        {
            serializedObject.Update();
            
            if (nodeDeleteIndex >= 0)
            {
                bindNodes.DeleteArrayElementAtIndex(nodeDeleteIndex);
                nodeDeleteIndex = -1;
            }

            rlBindNodeList.DoLayoutList();
            serializedObject.ApplyModifiedProperties();
        }

        private void DrawBoneNodes()
        {
            serializedObject.Update();
            
            if (nodeDeleteIndex >= 0)
            {
                boneNodes.DeleteArrayElementAtIndex(nodeDeleteIndex);
                nodeDeleteIndex = -1;
            }

            rlBoneNodeList.DoLayoutList();
            serializedObject.ApplyModifiedProperties();
        }

        private void DrawRendererNodes()
        {
            serializedObject.Update();

            if (nodeDeleteIndex >= 0)
            {
                smRendererNodes.DeleteArrayElementAtIndex(nodeDeleteIndex);
                nodeDeleteIndex = -1;
            }

            rlSMRendererNodeList.DoLayoutList();
            serializedObject.ApplyModifiedProperties();
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

            internal static GUIContent BindNodeTitle = new GUIContent("Bind Nodes");
            internal static GUIContent BoneNodeTitle = new GUIContent("Bone Nodes");
            internal static GUIContent RendererNodeTitle = new GUIContent("Renderer Nodes");

            internal static GUIContent AutoFindNodeBtnContent = new GUIContent("A", "Auto find node");
            internal static GUIContent ClearNodeBtnContent = new GUIContent("C", "Clear all nodes");
            internal static GUIContent DeleteNodeBtnContent = new GUIContent("D", "Delete current node");
        }
    }
}
