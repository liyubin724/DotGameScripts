using Dot.Entity.Node;
using DotEditor.GUIExtension;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DotEditor.Entity.Node
{
    [CustomEditor(typeof(NodeBehaviour))]
    public class NodeBehaviourEditor : Editor
    {
        private NodeBehaviour nodeBehaviour = null;
        
        void OnEnable()
        {
            nodeBehaviour = (NodeBehaviour)target;
        }

        private Vector2 scrollPos = Vector2.zero;

        public override void OnInspectorGUI()
        {
            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
            {
                DrawNodeDataArray("Bind Node", NodeType.BindNode, nodeBehaviour.bindNodes, () =>
                {
                    nodeBehaviour.bindNodes = new NodeData[0];

                    EditorGUIUtility.ExitGUI();
                    Repaint();
                }, () =>
                {
                    List<NodeData> datas = new List<NodeData>(nodeBehaviour.bindNodes);
                    NodeData data = new NodeData();
                    data.nodeType = NodeType.BindNode;
                    datas.Add(data);
                    nodeBehaviour.bindNodes = datas.ToArray();

                    EditorGUIUtility.ExitGUI();
                    Repaint();
                }, (index) =>
                {
                    List<NodeData> datas = new List<NodeData>(nodeBehaviour.bindNodes);
                    datas.RemoveAt(index);
                    nodeBehaviour.bindNodes = datas.ToArray();

                    EditorGUIUtility.ExitGUI();
                    Repaint();
                }, null);

                DrawNodeDataArray("Renderer Node", NodeType.SMRendererNode, nodeBehaviour.smRendererNodes, null, null, null, ()=>
                {
                    List<NodeData> datas = new List<NodeData>();

                    GameObject gObj = nodeBehaviour.gameObject;
                    SkinnedMeshRenderer[] renderers = gObj.GetComponentsInChildren<SkinnedMeshRenderer>(true);
                    foreach(var renderer in renderers)
                    {
                        NodeData data = new NodeData();
                        data.nodeType = NodeType.SMRendererNode;
                        data.name = renderer.name;
                        data.renderer = renderer;

                        datas.Add(data);
                    }

                    nodeBehaviour.smRendererNodes = datas.ToArray();

                    EditorGUIUtility.ExitGUI();
                    Repaint();
                });

                DrawNodeDataArray("Bone Node", NodeType.BindNode, nodeBehaviour.boneNodes, null, null, null, () =>
                {
                    List<NodeData> datas = new List<NodeData>();

                    GameObject gObj = nodeBehaviour.gameObject;
                    Transform[] transforms = gObj.GetComponentsInChildren<Transform>(true);
                    foreach (var transform in transforms)
                    {
                        NodeData data = new NodeData();
                        data.nodeType = NodeType.BoneNode;
                        data.name = transform.name;
                        data.transform = transform;

                        datas.Add(data);
                    }

                    nodeBehaviour.boneNodes = datas.ToArray();

                    EditorGUIUtility.ExitGUI();
                    Repaint();
                });
            }
            EditorGUILayout.EndScrollView();

            
        }

        private void DrawNodeDataArray(string header,NodeType nodeType,NodeData[] datas,
            Action clearAction,Action addAction,Action<int> deleteAction,Action findAction)
        {
            EditorGUILayout.BeginVertical(EGUIStyles.BoxStyle);
            {
                EditorGUILayout.LabelField(GUIContent.none, EditorStyles.toolbar, GUILayout.ExpandWidth(true));
                Rect lastRect = GUILayoutUtility.GetLastRect();
                EditorGUI.LabelField(lastRect, header, EGUIStyles.BoldLabelStyle);

                if(clearAction!=null)
                {
                    Rect clearBtnRect = new Rect(lastRect.x + lastRect.width - 40, lastRect.y, 40, lastRect.height);
                    if (GUI.Button(clearBtnRect, "C", EditorStyles.toolbarButton))
                    {
                        clearAction();
                        EditorGUIUtility.ExitGUI();
                    }
                }
                

                for (int i = 0; i < datas.Length; ++i)
                {
                    EditorGUILayout.BeginHorizontal();
                    {
                        EditorGUILayout.BeginVertical();
                        {
                            DrawNodeData(datas[i]);
                        }
                        EditorGUILayout.EndVertical();
                        if(deleteAction!=null)
                        {
                            if (GUILayout.Button("-", GUILayout.Width(20)))
                            {
                                deleteAction(i);
                                EditorGUIUtility.ExitGUI();
                            }
                        }
                    }
                    EditorGUILayout.EndHorizontal();

                    EGUILayout.DrawHorizontalLine();
                }

                if(addAction!=null || findAction!=null)
                {
                    EditorGUILayout.BeginHorizontal();
                    {
                        GUILayout.FlexibleSpace();
                        if(addAction!=null)
                        {
                            if (GUILayout.Button("+",GUILayout.Width(30)))
                            {
                                addAction();
                            }
                        }
                        if(findAction!=null)
                        {
                            if (GUILayout.Button("\u21BB", GUILayout.Width(30)))
                            {
                                findAction();
                            }
                        }
                    }
                    EditorGUILayout.EndHorizontal();
                }
            }
            EditorGUILayout.EndVertical();
        }

        private void DrawNodeData(NodeData data)
        {
            EditorGUI.BeginDisabledGroup(data.nodeType != NodeType.BindNode);
            {
                data.name = EditorGUILayout.TextField("name", data.name);
                if(data.nodeType == NodeType.SMRendererNode)
                {
                    data.renderer = (SkinnedMeshRenderer)EditorGUILayout.ObjectField("renderer", data.renderer, typeof(SkinnedMeshRenderer), true);
                }else
                {
                    data.transform = (Transform)EditorGUILayout.ObjectField("transform", data.transform, typeof(Transform), true);
                }
            }
            EditorGUI.EndDisabledGroup();
        }
    }
}
