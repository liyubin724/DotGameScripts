using Dot.Entity.Avatar;
using DotEditor.Core.EGUI;
using DotEditor.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using static DotEditor.Entity.Avatar.AvatarCreatorData;

namespace DotEditor.Entity.Avatar
{
    public class AvatarCreatorWindow : EditorWindow
    {
        [MenuItem("Game/Entity/Avatar Creator Window")]
        static void ShowWin()
        {
            var win = EditorWindow.GetWindow<AvatarCreatorWindow>();
            win.titleContent = Contents.WinTitleContent;
            win.Show();
        }

        private int CREATOR_DATA_ITEM_WIDTH = 120;
        private int CREATOR_DATA_ITEM_HEIGHT = 40;

        private List<AvatarCreatorData> creatorDatas = new List<AvatarCreatorData>();
        private AvatarCreatorData selectedData = null;

        private AvatarSkeletonCreatorDrawer skeletonDrawer = null;
        private AvatarPartCreatorDrawer partDrawer = null;
        private void OnEnable()
        {
            skeletonDrawer = new AvatarSkeletonCreatorDrawer(Repaint);
            partDrawer = new AvatarPartCreatorDrawer(Repaint);

            LoadDatas();
        }

        private Vector2 creatorDataScrollPos = Vector2.zero;
        private int toolbarSelectedIndex = 0;
        private void OnGUI()
        {
            EditorGUILayout.BeginVertical();
            {
                DrawToolbar();

                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.BeginVertical(EditorStyles.helpBox, GUILayout.ExpandHeight(true), GUILayout.Width(CREATOR_DATA_ITEM_WIDTH));
                    {
                        EditorGUILayout.LabelField(Contents.CreatorDataTitleContent, Styles.boldLabelCenterStyle);
                        creatorDataScrollPos = EditorGUILayout.BeginScrollView(creatorDataScrollPos, EditorStyles.helpBox, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
                        {
                            DrawCreatorDataList();
                        }
                        EditorGUILayout.EndScrollView();
                    }
                    EditorGUILayout.EndVertical();
                    

                    EditorGUILayout.BeginVertical(EditorStyles.helpBox, GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));
                    {
                        if(selectedData!=null)
                        {
                            EditorGUILayout.LabelField(selectedData.dataName, Styles.boldLabelCenterStyle);
                            EditorGUILayout.Space();

                            DrawCreatorData();
                            toolbarSelectedIndex = GUILayout.Toolbar(toolbarSelectedIndex, Contents.ToolbarContents,GUILayout.Height(40));
                            if(toolbarSelectedIndex == 0)
                            {
                                skeletonDrawer.OnGUI();
                            }else if(toolbarSelectedIndex == 1)
                            {
                                partDrawer.OnGUI();
                            }
                        }
                    }
                    EditorGUILayout.EndVertical();
                }
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndVertical();

            if(GUI.changed && selectedData!=null)
            {
                EditorUtility.SetDirty(selectedData);
            }
        }

        private void DrawToolbar()
        {
            GUILayout.BeginHorizontal("toolbar", GUILayout.ExpandWidth(true));
            {
                if (GUILayout.Button(Contents.RefreshBtnContent, EditorStyles.toolbarButton, GUILayout.Width(60)))
                {
                    LoadDatas();
                }
                if (GUILayout.Button(Contents.NewBtnContent, EditorStyles.toolbarButton, GUILayout.Width(60)))
                {
                    CreateData();
                }
                if (GUILayout.Button(Contents.DeleteBtnContent, EditorStyles.toolbarButton, GUILayout.Width(60)))
                {
                    DeleteData();
                }
                GUILayout.FlexibleSpace();
                if (GUILayout.Button(Contents.ExportAllBtnContent, EditorStyles.toolbarButton, GUILayout.Width(60)))
                {
                    if (creatorDatas != null && creatorDatas.Count > 0)
                    {
                        foreach (var data in creatorDatas)
                        {
                            AvatarEditorUtil.CreateAvatar(data);
                        }

                        EditorUtility.DisplayDialog(Contents.FinishedStr, Contents.ExportAllFinishedStr, Contents.OkBtnStr);
                    }
                }
                if (GUILayout.Button(Contents.PreviewBtnContent, EditorStyles.toolbarButton, GUILayout.Width(120)))
                {
                    if(selectedData!=null)
                    {
                        AvatarEditorUtil.CreatePreview(selectedData);
                    }
                }
            }
            GUILayout.EndHorizontal();
        }

        private void DrawCreatorDataList()
        {
            EditorGUILayout.BeginVertical();
            {
                foreach(var data in creatorDatas)
                {
                    Color color = GUI.backgroundColor;
                    if(data == selectedData)
                    {
                        GUI.backgroundColor = Color.blue;
                    }

                    if(GUILayout.Button(data.dataName,GUILayout.Height(CREATOR_DATA_ITEM_HEIGHT)))
                    {
                        ChangeSelected(data);
                    }

                    GUI.backgroundColor = color;
                }
            }
            EditorGUILayout.EndVertical();
        }

        private void DrawCreatorData()
        {
            EditorGUIUtil.BeginLabelWidth(80);
            {
                selectedData.dataName = EditorGUILayout.TextField(Contents.DataNameContent, selectedData.dataName);
                selectedData.isEnable = EditorGUILayout.Toggle(Contents.IsEnableContent, selectedData.isEnable);
            }
            EditorGUIUtil.EndLableWidth();
        }

        private void LoadDatas()
        {
            creatorDatas = AvatarEditorUtil.FindCreatorDatas();
            selectedData = null;

            if(creatorDatas.Count>0)
            {
               ChangeSelected(creatorDatas[0]);
            }
        }

        private void CreateData()
        {
            string assetPath = EditorUtility.SaveFilePanel(Contents.SaveStr,
                AvatarEditorUtil.CREATOR_DATA_DIR,
                AvatarEditorUtil.CREATOR_DATA_DEFAULT_NAME, "asset");
            if(!string.IsNullOrEmpty(assetPath))
            {
                assetPath = PathUtil.GetAssetPath(assetPath);
                assetPath = AssetDatabase.GenerateUniqueAssetPath(assetPath);

                AvatarCreatorData data = ScriptableObject.CreateInstance<AvatarCreatorData>();
                data.dataName = Path.GetFileNameWithoutExtension(assetPath);
                AssetDatabase.CreateAsset(data, assetPath);
                AssetDatabase.ImportAsset(assetPath);

                creatorDatas.Add(data);
                ChangeSelected(data);
            }
        }

        private void DeleteData()
        {
            if(selectedData!=null)
            {
                int index = creatorDatas.IndexOf(selectedData);

                creatorDatas.RemoveAt(index);
                string assetPath = AssetDatabase.GetAssetPath(selectedData);

                if(index>=creatorDatas.Count)
                {
                    index = creatorDatas.Count - 1;
                }

                if(index>=0)
                {
                    ChangeSelected(creatorDatas[index]);
                }else
                {
                    ChangeSelected(null);
                }
                
                AssetDatabase.DeleteAsset(assetPath);
            }
        }

        private void ChangeSelected(AvatarCreatorData data)
        {
            selectedData = data;

            if(selectedData!=null)
            {
                partDrawer.SetData(data.partCreatorDatas);
                skeletonDrawer.SetData(data.skeletonCreatorDatas);
            }else
            {
                partDrawer.SetData(null);
                skeletonDrawer.SetData(null);
            }

            Repaint();
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
            internal static GUIContent WinTitleContent = new GUIContent("Avatar Creator");
            
            internal static GUIContent RefreshBtnContent = new GUIContent("Refresh");
            internal static GUIContent NewBtnContent = new GUIContent("New");
            internal static GUIContent DeleteBtnContent = new GUIContent("Delete");
            internal static GUIContent ExportAllBtnContent = new GUIContent("Export All");
            internal static GUIContent PreviewBtnContent = new GUIContent("Create Preview");

            internal static GUIContent[] ToolbarContents = new GUIContent[]
            {
                new GUIContent("Skeleton Creator"),
                new GUIContent("Part Creator"),
            };

            internal static GUIContent CreatorDataTitleContent = new GUIContent("Data List");
            internal static GUIContent DataNameContent = new GUIContent("Data Name");
            internal static string SkeletonSavedDirStr = "Skeleton Dir";
            internal static GUIContent IsEnableContent = new GUIContent("Is Enable");
            internal static GUIContent FBXPrefabContent = new GUIContent("FBX Prefab");

            internal static string SaveStr = "Save";

            internal static string OkBtnStr = "OK";
            internal static string FinishedStr = "Finished";
            internal static string ExportAllFinishedStr = "Export all finished";

        }
    }
}
