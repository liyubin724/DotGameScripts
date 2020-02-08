using DotEditor.Core.EGUI;
using DotEditor.Util;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using static DotEditor.Entity.Avatar.AvatarSkeletonCreatorData;

namespace DotEditor.Entity.Avatar
{
    public class AvatarSkeletonCreatorWindow : EditorWindow
    {
        private static readonly string SKELETON_CONFIG_DIR = "Assets/Tools/Entity/Avatar";
        private static readonly string SKELETON_CONFIG_NAME = "avatar_skeleton_config";

        private static float DATA_LIST_WIDTH = 120;
        internal static void ShowWin()
        {
            var win = EditorWindow.GetWindow<AvatarSkeletonCreatorWindow>();
            win.titleContent = Contents.WinTitleContent;
            win.Show();
        }

        private List<AvatarSkeletonCreatorData> creatorDatas = null;

        AvatarSkeletonCreatorData selectedData = null;
        private ReorderableList dataRList = null;
        private void OnEnable()
        {
            LoadDatas();
        }

        private void LoadDatas()
        {
            creatorDatas = AvatarSkeletonCreatorUtil.FindAllData();
            dataRList = null;
            selectedData = null;
        }

        private Rect dataTotalRect = Rect.zero;
        private void OnGUI()
        {
            DrawToolbar();

            Rect lastRect = GUILayoutUtility.GetRect(new GUIContent(""), EditorStyles.label, GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));

            if(Event.current.type == EventType.Repaint)
            {
                dataTotalRect = lastRect;
            }

            Rect dataListRect = dataTotalRect;
            dataListRect.width = DATA_LIST_WIDTH;
            DrawDataList(dataListRect);

            Rect dataRect = dataTotalRect;
            dataRect.x += dataListRect.width;
            dataRect.width -= dataListRect.width;
            DrawData(dataRect);
        }

        private void DrawToolbar()
        {
            GUILayout.BeginHorizontal("toolbar", GUILayout.ExpandWidth(true));
            {
                if(GUILayout.Button(Contents.RefreshBtnContent,EditorStyles.toolbarButton,GUILayout.Width(60)))
                {
                    LoadDatas();
                }
                if (GUILayout.Button(Contents.NewBtnContent, EditorStyles.toolbarButton, GUILayout.Width(60)))
                {
                    CreateData();
                }
                GUILayout.FlexibleSpace();
            }
            GUILayout.EndHorizontal();
        }

        private Vector2 dataListScrollPos = Vector2.zero;
        private void DrawDataList(Rect rect)
        {
            GUILayout.BeginArea(rect);
            {
                dataListScrollPos = GUILayout.BeginScrollView(dataListScrollPos, EditorStyles.helpBox,GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));
                {
                    foreach (var data in creatorDatas)
                    {
                        Color color = GUI.backgroundColor;
                        if(selectedData!=null && data == selectedData)
                        {
                            GUI.backgroundColor = Color.blue;
                        }
                        if (GUILayout.Button(data.creatorName, GUILayout.ExpandWidth(true), GUILayout.Height(40)))
                        {
                            OnDataSelected(data);
                        }
                        GUI.backgroundColor = color;
                    }
                }
                GUILayout.EndScrollView();
            }
            GUILayout.EndArea();
        }

        private Vector2 dataScrollPos = Vector2.zero;
        private void DrawData(Rect rect)
        {
            GUILayout.BeginArea(rect);
            {
                dataScrollPos = GUILayout.BeginScrollView(dataScrollPos, EditorStyles.helpBox, GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));
                {
                    if (selectedData != null)
                    {
                        selectedData.creatorName = EditorGUILayout.TextField(Contents.CreatorNameContent, selectedData.creatorName);
                        selectedData.savedAssetDir = EditorGUILayoutUtil.DrawAssetFolderSelection(Contents.SavedAssetDirContent, selectedData.savedAssetDir);
                        EditorGUILayout.Space();
                        dataRList.DoLayoutList();
                        EditorGUILayout.Space();
                        if (GUILayout.Button(Contents.CreateSkeletonContent, GUILayout.Height(40)))
                        {
                            AvatarSkeletonCreatorUtil.CreateSkeleton(selectedData);
                        }
                    }
                }
                GUILayout.EndScrollView();
            }
            GUILayout.EndArea();
        }

        private void CreateData()
        {
            AvatarSkeletonCreatorData data = ScriptableObject.CreateInstance<AvatarSkeletonCreatorData>();
            string dataPath = EditorUtility.SaveFilePanel("Save", PathUtil.GetDiskPath(SKELETON_CONFIG_DIR), SKELETON_CONFIG_NAME, "asset");
            if(!string.IsNullOrEmpty(dataPath))
            {
                string assetPath = PathUtil.GetAssetPath(dataPath);
                AssetDatabase.CreateAsset(data, assetPath);
                AssetDatabase.ImportAsset(assetPath);

                creatorDatas.Add(data);

                OnDataSelected(data);
            }
        }

        private void OnDataSelected(AvatarSkeletonCreatorData data)
        {
            selectedData = data;

            dataRList = new ReorderableList(selectedData.datas, typeof(AvatarSkeletonData), true, true, true, true);
            dataRList.elementHeight = EditorGUIUtility.singleLineHeight * 3;
            dataRList.drawHeaderCallback = (rect) =>
            {
                EditorGUI.LabelField(rect, Contents.FBXListTitleContent);
            };
            dataRList.drawElementCallback = (rect, index, isActive, isFocused) =>
            {
                AvatarSkeletonData asData = selectedData.datas[index];

                Rect drawRect = rect;
                drawRect.height = EditorGUIUtility.singleLineHeight;
                asData.isEnable = EditorGUI.Toggle(drawRect, Contents.FBXContent ,asData.isEnable);
                drawRect.y += drawRect.height;
                asData.fbxPrefab = (GameObject)EditorGUI.ObjectField(drawRect,Contents.IsEnableContent, asData.fbxPrefab, typeof(GameObject), false);
            };
            dataRList.onAddCallback = (list) =>
            {
                list.list.Add(new AvatarSkeletonData());
            };

            Repaint();
        }

        class Contents
        {
            internal static GUIContent WinTitleContent = new GUIContent("Skeleton Creator");
            internal static GUIContent FBXListTitleContent = new GUIContent("FBX List");

            internal static GUIContent RefreshBtnContent = new GUIContent("Refresh");
            internal static GUIContent NewBtnContent = new GUIContent("New");

            internal static GUIContent CreateSkeletonContent = new GUIContent("Create Skeleton");

            internal static GUIContent CreatorNameContent = new GUIContent("Creator Name");
            internal static string SavedAssetDirContent = "Saved Asset Dir";

            internal static GUIContent IsEnableContent = new GUIContent("Is Enable");
            internal static GUIContent FBXContent = new GUIContent("FBX");
        }
    }
}
