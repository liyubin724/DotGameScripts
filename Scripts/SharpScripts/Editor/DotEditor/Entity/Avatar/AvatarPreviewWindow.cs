using Dot.Entity.Avatar;
using Dot.Entity.Node;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DotEditor.Entity.Avatar
{
    public class AvatarPreviewWindow : EditorWindow
    {
        [MenuItem("Game/Entity/Avatar Preview Window")]
        static void ShowWin()
        {
            var win = EditorWindow.GetWindow<AvatarPreviewWindow>();
            win.titleContent = Contents.WinTitleContent;
            win.Show();
        }

        private List<AvatarPreviewData> previewDatas = new List<AvatarPreviewData>();
        private GameObject skeletonPrefab = null;

        private Dictionary<AvatarPartType, AvatarPartData> partDataDic = new Dictionary<AvatarPartType, AvatarPartData>();
        private Dictionary<AvatarPartType, AvatarPartInstance> partInstanceDic = new Dictionary<AvatarPartType, AvatarPartInstance>();

        private GameObject skeletonInstance = null;
        private void OnEnable()
        {
            previewDatas = AvatarEditorUtil.FindPreviewDatas();
            foreach(var data in previewDatas)
            {
                for(int i =data.skeletonPefabs.Count-1;i>=0;--i)
                {
                    if(data.skeletonPefabs[i] == null)
                    {
                        data.skeletonPefabs.RemoveAt(i);
                    }
                }

                for(int i = data.partDatas.Count-1;i>=0;--i)
                {
                    if(data.partDatas[i] == null)
                    {
                        data.partDatas.RemoveAt(i);
                    }
                }

                EditorUtility.SetDirty(data);
            }

            if(previewDatas.Count>0)
            {
                ChangeSelected(previewDatas[0]);
            }
        }

        private void OnDestroy()
        {
            UnloadSkeleton();
        }

        private Vector2 dataListScrollPos = Vector2.zero;
        private Vector2 skeletonScrollPos = Vector2.zero;
        private Vector2 partScrollPos = Vector2.zero;

        private AvatarPreviewData selectedData = null;
        private void OnGUI()
        {
            EditorGUILayout.BeginHorizontal(EditorStyles.helpBox, GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));
            {
                EditorGUILayout.BeginVertical(EditorStyles.helpBox, GUILayout.Width(120), GUILayout.ExpandHeight(true));
                {
                    EditorGUILayout.LabelField(Contents.PreviewDataTitleContent, Styles.boldLabelCenterStyle);
                    dataListScrollPos = EditorGUILayout.BeginScrollView(dataListScrollPos, EditorStyles.helpBox, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
                    {
                        EditorGUILayout.BeginVertical();
                        {
                            DrawDataList();
                        }
                        EditorGUILayout.EndVertical();
                    }
                    EditorGUILayout.EndScrollView();
                }
                EditorGUILayout.EndVertical();
                

                float skeletonWidth = 0.5f * (position.width - 120);
                EditorGUILayout.BeginVertical(EditorStyles.helpBox, GUILayout.Width(skeletonWidth), GUILayout.ExpandHeight(true));
                {
                    EditorGUILayout.LabelField(Contents.SkeletonTitleContent, Styles.boldLabelCenterStyle);
                    EditorGUILayout.Space();
                    skeletonScrollPos = EditorGUILayout.BeginScrollView(skeletonScrollPos, EditorStyles.helpBox, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
                    {
                        DrawSkeletonList();
                    }
                    EditorGUILayout.EndScrollView();
                }
                EditorGUILayout.EndVertical();

                EditorGUILayout.BeginVertical(EditorStyles.helpBox, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
                {
                    EditorGUILayout.LabelField(Contents.PartTitleContent, Styles.boldLabelCenterStyle);
                    EditorGUILayout.Space();
                    partScrollPos = EditorGUILayout.BeginScrollView(partScrollPos, EditorStyles.helpBox, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
                    {
                        EditorGUILayout.BeginVertical(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
                        {
                            DrawPartList();
                        }
                        EditorGUILayout.EndVertical();
                    }
                    EditorGUILayout.EndScrollView();
                }
                EditorGUILayout.EndVertical();

            }
            EditorGUILayout.EndHorizontal();
        }

        private void DrawDataList()
        {
            foreach (var data in previewDatas)
            {
                Color color = GUI.backgroundColor;
                if (data == selectedData)
                {
                    GUI.backgroundColor = Color.blue;
                }
                if (GUILayout.Button(data.dataName, GUILayout.Height(40)))
                {
                    ChangeSelected(data);
                }
                GUI.backgroundColor = color;
            }
        }

        private void DrawSkeletonList()
        {
            if(selectedData!=null)
            {
                for(int i =0;i<selectedData.skeletonPefabs.Count;++i)
                {
                    var skeleton = selectedData.skeletonPefabs[i];
                    EditorGUILayout.BeginHorizontal();
                    {
                        bool isSelected = skeleton == skeletonPrefab;

                        bool newIsSelected = EditorGUILayout.ToggleLeft(i.ToString()+" "+skeleton.name,isSelected);
                        if(newIsSelected!=isSelected)
                        {
                            UnloadSkeleton();
                            if(newIsSelected)
                            {
                                skeletonPrefab = skeleton;
                                LoadSkeleton();
                            }
                        }
                        EditorGUILayout.ObjectField(skeleton, typeof(GameObject), false);
                    }
                    EditorGUILayout.EndHorizontal();
                }
            }
        }

        private void DrawPartList()
        {
            if(selectedData!=null)
            {
                for(int i = (int)AvatarPartType.None+1;i<(int)AvatarPartType.Max;++i)
                {
                    AvatarPartType partType = (AvatarPartType)i;
                    EditorGUILayout.LabelField(partType.ToString(), Styles.boldLabelCenterStyle,GUILayout.ExpandWidth(true));
                    EditorGUILayout.Space();
                    for (int j = 0;j<selectedData.partDatas.Count;++j)
                    {
                        var partData = selectedData.partDatas[j];
                        if(partData.partType == partType)
                        {
                            bool isSelected = false;
                            if (partDataDic.TryGetValue(partData.partType, out AvatarPartData pData))
                            {
                                isSelected = pData == partData;
                            }

                            EditorGUILayout.BeginHorizontal();
                            {
                                bool newIsSelected = EditorGUILayout.ToggleLeft(partData.name, isSelected);
                                if (newIsSelected != isSelected)
                                {
                                    if(!newIsSelected)
                                    {
                                        DisassemblePart(partData.partType);
                                    }else
                                    {
                                        AssemblePart(partData);
                                    }
                                }
                                EditorGUILayout.ObjectField(partData, typeof(AvatarPartData), false);
                            }
                            EditorGUILayout.EndHorizontal();
                        }
                    }
                    EditorGUILayout.Space();
                }
            }
        }

        private void ChangeSelected(AvatarPreviewData data)
        {
            if(data != selectedData)
            {
                if(selectedData!=null)
                {
                    UnloadSkeleton();
                }
                selectedData = data;
                LoadSkeleton();
            }
        }

        private void LoadSkeleton()
        {
            if(selectedData!=null)
            {
                partDataDic.Clear();
                partInstanceDic.Clear();

                if (skeletonPrefab!=null)
                {
                    skeletonInstance = GameObject.Instantiate(skeletonPrefab);

                    SceneView.lastActiveSceneView.LookAt(skeletonInstance.transform.position);
                }
            }
        }

        private void UnloadSkeleton()
        {
            partDataDic.Clear();
            partInstanceDic.Clear();

            if (skeletonInstance != null)
            {
                GameObject.DestroyImmediate(skeletonInstance);
            }
            skeletonInstance = null;
            skeletonPrefab = null;
        }

        private void AssemblePart(AvatarPartData partData)
        {
            if(skeletonInstance == null)
            {
                return;
            }

            DisassemblePart(partData.partType);

            partDataDic[partData.partType] = partData;

            NodeBehaviour nodeBehaviour = skeletonInstance.GetComponent<NodeBehaviour>();
            AvatarPartInstance pInstance = AvatarUtil.AssembleAvatarPart(nodeBehaviour, partData);
            
            partInstanceDic.Add(partData.partType, pInstance);
        }

        private void DisassemblePart(AvatarPartType partType)
        {
            if (skeletonInstance == null)
            {
                return;
            }

            NodeBehaviour nodeBehaviour = skeletonInstance.GetComponent<NodeBehaviour>();
            if (partInstanceDic.TryGetValue(partType, out AvatarPartInstance pInstance))
            {
                if (nodeBehaviour != null)
                {
                    AvatarUtil.DisassembleAvatarPart(pInstance,true);
                }

                partInstanceDic.Remove(partType);
            }
            partDataDic.Remove(partType);
        }


        class Styles
        {
            internal static GUIStyle boldLabelCenterStyle = null;
            static Styles()
            {
                boldLabelCenterStyle = new GUIStyle(EditorStyles.label);
                boldLabelCenterStyle.fontSize = 18;
                boldLabelCenterStyle.fontStyle = FontStyle.Bold;
                boldLabelCenterStyle.alignment = TextAnchor.MiddleCenter;
                boldLabelCenterStyle.normal.textColor = Color.cyan;
                boldLabelCenterStyle.normal.background = EditorStyles.toolbar.normal.background;
                boldLabelCenterStyle.fixedHeight = 22;
            }
        }

        class Contents
        {
            internal static GUIContent WinTitleContent = new GUIContent("Avatar Preview");
            internal static GUIContent PreviewDataTitleContent = new GUIContent("Data List");
            internal static GUIContent SkeletonTitleContent = new GUIContent("Skeleton List");
            internal static GUIContent PartTitleContent = new GUIContent("Part List");
        }
    }
}
