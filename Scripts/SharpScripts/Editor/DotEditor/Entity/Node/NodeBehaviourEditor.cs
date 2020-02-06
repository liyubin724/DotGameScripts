using Dot.Entity.Node;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace DotEditor.Entity.Node
{
    public enum NodeErrorType
    {
        None = 0,
        NameRepeated,
        NameEmpty,
    }

    [CustomEditor(typeof(NodeBehaviour))]
    public class NodeBehaviourEditor : Editor
    {
        private SerializedProperty bindNodes;
        private SerializedProperty boneNodes;
        private SerializedProperty smRendererNodes;

        private bool isBindNodeVisible = true;
        private bool isBoneNodeVisible = false;
        private bool isRendererNodeVisible = false;

        private ReorderableList rlBindNodeList = null;
        private ReorderableList rlBoneNodeList = null;
        private ReorderableList rlSMRendererNodeList = null;

        private Dictionary<NodeErrorType, string> errMsgDic = new Dictionary<NodeErrorType, string>()
        {
            {NodeErrorType.NameEmpty,"The name of the node is empty.index = {0}" },
            {NodeErrorType.NameRepeated,"The name of the node is repeated.name = {0}" },
        };
        void OnEnable()
        {
            bindNodes = serializedObject.FindProperty("bindNodes");
            boneNodes = serializedObject.FindProperty("boneNodes");
            smRendererNodes = serializedObject.FindProperty("smRendererNodes");

            rlBindNodeList = new ReorderableList(serializedObject, bindNodes, true, true, true, true);
            rlBindNodeList.elementHeight = EditorGUIUtility.singleLineHeight * 5;
            rlBindNodeList.drawHeaderCallback = (rect) =>
            {
                EditorGUI.LabelField(rect, "Bind Nodes");
            };
            rlBindNodeList.drawElementCallback = (rect, index, isActive, isFocused) =>
            {
               SerializedProperty nodeData =  bindNodes.GetArrayElementAtIndex(index);
                DrawNodeData(rect, nodeData);
            };
            rlBindNodeList.onAddCallback = (list) =>
            {
                bindNodes.InsertArrayElementAtIndex(bindNodes.arraySize);
                SerializedProperty nodeData = bindNodes.GetArrayElementAtIndex(bindNodes.arraySize - 1);
                nodeData.FindPropertyRelative("nodeType").intValue = (int)NodeType.BindNode;
            };

            rlBoneNodeList = new ReorderableList(serializedObject, boneNodes, false, true, false, false);
            rlBoneNodeList.elementHeight = EditorGUIUtility.singleLineHeight * 3;
            rlBoneNodeList.drawHeaderCallback = (rect) =>
            {
                EditorGUI.LabelField(rect, "Bone Nodes");
            };
            rlBoneNodeList.drawElementCallback = (rect, index, isActive, isFocused) =>
            {
                SerializedProperty nodeData = boneNodes.GetArrayElementAtIndex(index);
                DrawNodeData(rect, nodeData);
            };

            rlSMRendererNodeList = new ReorderableList(serializedObject, smRendererNodes, false, true, false, false);
            rlSMRendererNodeList.elementHeight = EditorGUIUtility.singleLineHeight * 3;
            rlSMRendererNodeList.drawHeaderCallback = (rect) =>
            {
                EditorGUI.LabelField(rect, "Renderer Nodes");
            };
            rlSMRendererNodeList.drawElementCallback = (rect, index, isActive, isFocused) =>
            {
                SerializedProperty nodeData = smRendererNodes.GetArrayElementAtIndex(index);
                DrawNodeData(rect,nodeData);
            };
        }

        private void DrawNodeData(Rect rect,SerializedProperty nodeData)
        {
            NodeType nodeType = (NodeType)nodeData.FindPropertyRelative("nodeType").intValue;
            EditorGUI.BeginDisabledGroup(nodeType == NodeType.BoneNode || nodeType == NodeType.SMRendererNode);
            {
                Rect drawRect = rect;
                drawRect.height = EditorGUIUtility.singleLineHeight;

                EditorGUI.PropertyField(drawRect, nodeData.FindPropertyRelative("name"));
                if(nodeType == NodeType.BindNode || nodeType == NodeType.BoneNode)
                {
                    drawRect.y += drawRect.height;
                    EditorGUI.PropertyField(drawRect, nodeData.FindPropertyRelative("transform"));
                }else  if(nodeType == NodeType.SMRendererNode)
                {
                    drawRect.y += drawRect.height;
                    EditorGUI.PropertyField(drawRect, nodeData.FindPropertyRelative("renderer"));
                }
                if(nodeType == NodeType.BindNode)
                {
                    drawRect.y += drawRect.height;
                    EditorGUI.PropertyField(drawRect, nodeData.FindPropertyRelative("positionOffset"));
                    drawRect.y += drawRect.height;
                    EditorGUI.PropertyField(drawRect, nodeData.FindPropertyRelative("rotationOffset"));
                }
            }
            EditorGUI.EndDisabledGroup();
        }
        
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            if(CheckNodeProperty(bindNodes,out string errMsg1) != NodeErrorType.None)
            {
                EditorGUILayout.HelpBox(errMsg1, MessageType.Error);
            }
            isBindNodeVisible = EditorGUILayout.Toggle("Bind Node:", isBindNodeVisible);
            if(isBindNodeVisible)
            {
                rlBindNodeList.DoLayoutList();
            }

            if (CheckNodeProperty(boneNodes, out string errMsg2) != NodeErrorType.None)
            {
                EditorGUILayout.HelpBox(errMsg2, MessageType.Error);
            }
            isBoneNodeVisible = EditorGUILayout.Toggle("Bone Node:", isBoneNodeVisible);
            if(isBoneNodeVisible)
            {
                rlBoneNodeList.DoLayoutList();
            }

            if (CheckNodeProperty(smRendererNodes, out string errMsg3) != NodeErrorType.None)
            {
                EditorGUILayout.HelpBox(errMsg3, MessageType.Error);
            }
            isRendererNodeVisible = EditorGUILayout.Toggle("Renderer Node:", isRendererNodeVisible);
            if(isRendererNodeVisible)
            {
                rlSMRendererNodeList.DoLayoutList();
            }

            serializedObject.ApplyModifiedProperties();
        }

        private NodeErrorType CheckNodeProperty(SerializedProperty nodeProperty,out string errMsg)
        {
            NodeErrorType errorType = NodeErrorType.None;
            errMsg = null;
            List<string> names = new List<string>();
            for(int i =0;i<nodeProperty.arraySize;++i)
            {
                SerializedProperty nodeData = nodeProperty.GetArrayElementAtIndex(i);
                string name = nodeData.FindPropertyRelative("name").stringValue;
                if(string.IsNullOrEmpty(name))
                {
                    errorType = NodeErrorType.NameEmpty;
                    errMsg = string.Format(errMsgDic[errorType],i);
                    break;
                }else if(names.IndexOf(name) >= 0)
                {
                    errorType = NodeErrorType.NameRepeated;
                    errMsg = string.Format(errMsgDic[errorType],name);
                    break;
                }else
                {
                    names.Add(name);
                }
            }

            return errorType;
        }
    }
}
