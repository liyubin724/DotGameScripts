using DotEditor.GUIExtension;
using DotEditor.GUIExtension.ListView;
using DotEditor.NativeDrawer;
using DotEditor.Utilities;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using static DotEditor.Entity.Avatar.AvatarCreatorData;

namespace DotEditor.Entity.Avatar
{
    public class AvatarCreatorWindow : EditorWindow
    {
        [MenuItem("Game/Entity/Avatar Creator")]
        static void ShowWin()
        {
            var win = EditorWindow.GetWindow<AvatarCreatorWindow>();
            win.titleContent = new GUIContent("Avatar Creator");
            win.Show();
        }

        private static int TOOLBAR_HEIGHT = 18;
        private static int DATA_LIST_WIDTH = 200;
        private static int LINE_THINKNESS = 1;

        private List<string> creatorDataFiles = new List<string>();
        private SimpleListView<string> dataListView;

        private AvatarCreatorData currentCreatorData = null;
        private NativeDrawerObject partOutputDataDrawer = null;
        private NativeDrawerObject skeletonCreatorDataDrawer = null;

        private List<PartCreatorData> selectedPartCreatorDatas = new List<PartCreatorData>();
        void OnEnable()
        {
            FindAllData();

            dataListView = new SimpleListView<string>(creatorDataFiles);
            dataListView.Header = "Data List";
            dataListView.OnItemSelected = OnListViewItemSelected;
            dataListView.OnDrawItem = (rect, data) =>
            {
                EditorGUI.LabelField(rect, Path.GetFileNameWithoutExtension(data), EGUIStyles.BoldLabelStyle);
            };
            dataListView.Reload();

            PartCreatorDataDrawer.IsPartSelected = (data) =>
            {
                return selectedPartCreatorDatas.Contains(data);
            };
            PartCreatorDataDrawer.PartSelectedChanged = (data, isSelected) =>
            {
                if(isSelected)
                {
                    foreach(var d in selectedPartCreatorDatas)
                    {
                        if(d.partType == data.partType)
                        {
                            selectedPartCreatorDatas.Remove(d);
                            break;
                        }
                    }
                    selectedPartCreatorDatas.Add(data);
                }else
                {
                    selectedPartCreatorDatas.Remove(data);
                }
            };
            PartCreatorDataDrawer.CreatePartBtnClick = (data) =>
            {
                CreatePart(data);
            };
        }

        void OnDisable()
        {
            PartCreatorDataDrawer.IsPartSelected = null;
            PartCreatorDataDrawer.PartSelectedChanged = null;
            PartCreatorDataDrawer.CreatePartBtnClick = null;
        }

        private void OnListViewItemSelected(string filePath)
        {
            selectedPartCreatorDatas.Clear();

            currentCreatorData = AssetDatabase.LoadAssetAtPath<AvatarCreatorData>(filePath);

            skeletonCreatorDataDrawer = new NativeDrawerObject(currentCreatorData.skeletonData)
            {
                IsShowScroll = true,
            };
            partOutputDataDrawer = new NativeDrawerObject(currentCreatorData.partOutputData)
            {
                IsShowScroll = true
            };

            Repaint();
        }

        private void FindAllData()
        {
            creatorDataFiles.Clear();

            string[] assetPaths = AssetDatabaseUtility.FindAssets<AvatarCreatorData>();
            foreach(var assetPath in assetPaths)
            {
                creatorDataFiles.Add(assetPath);
            }

            dataListView?.Reload();
        }

        void OnGUI()
        {
            Rect rect = new Rect(0, 0, position.width, position.height);

            Rect toolbarRect = new Rect(rect.x, rect.y, position.width, TOOLBAR_HEIGHT);
            EditorGUI.LabelField(toolbarRect, GUIContent.none, EditorStyles.toolbar);
            DrawToolbar(toolbarRect);

            Rect dataListRect = new Rect(rect.x+LINE_THINKNESS, rect.y + TOOLBAR_HEIGHT+LINE_THINKNESS,
                                                                DATA_LIST_WIDTH-LINE_THINKNESS*2, rect.height - TOOLBAR_HEIGHT - LINE_THINKNESS *2);
            dataListView.OnGUI(dataListRect);

            Rect skeletonRect = new Rect(dataListRect.x + dataListRect.width, dataListRect.y, (rect.width - dataListRect.width) * 0.4f, dataListRect.height);
            EGUI.DrawAreaLine(skeletonRect,Color.black);
            DrawSkeleton(skeletonRect);

            Rect partRect = new Rect(skeletonRect.x + skeletonRect.width, dataListRect.y, (rect.width - dataListRect.width) * 0.6f, dataListRect.height);
            EGUI.DrawAreaLine(partRect, Color.black);
            DrawParts(partRect);

            if(GUI.changed)
            {
                EditorUtility.SetDirty(currentCreatorData);
            }
        }

        private void DrawToolbar(Rect rect)
        {
            GUILayout.BeginArea(rect);
            {
                GUILayout.BeginHorizontal();
                {
                    if(EGUILayout.ToolbarButton("New"))
                    {
                        //var newData = EGUIUtility.CreateAsset<AvatarCreatorData>();
                        //if(newData!=null)
                        //{
                        //    creatorDataFiles.Add(AssetDatabase.GetAssetPath(newData));
                        //    dataListView.SetSelection(new int[] { creatorDataFiles.Count - 1 });
                        //    dataListView.Reload();
                        //}
                    }
                    EGUILayout.ToolbarButton("Delete");
                }
                GUILayout.EndHorizontal();
            }
            GUILayout.EndArea();
        }

        private void DrawSkeleton(Rect rect)
        {
            GUILayout.BeginArea(rect);
            {
                EditorGUILayout.BeginVertical();
                {
                    EGUILayout.DrawBoxHeader("Skeleton Data", EGUIStyles.BoxedHeaderCenterStyle, GUILayout.ExpandWidth(true));
                    if(currentCreatorData !=null && skeletonCreatorDataDrawer!=null)
                    {
                        skeletonCreatorDataDrawer.OnGUILayout();

                        SkeletonCreatorData skeletonCreatorData = currentCreatorData.skeletonData;

                        string targetPrefabPath = skeletonCreatorData.GetTargetPrefabPath();
                        GameObject targetPrefab = null;
                        if(!string.IsNullOrEmpty(targetPrefabPath))
                        {
                            targetPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(targetPrefabPath);
                        }
                        EditorGUILayout.Space();
                        EditorGUILayout.Space();

                        EditorGUILayout.ObjectField("Output", targetPrefab, typeof(GameObject), false);

                        EditorGUILayout.Space();

                        GUILayout.FlexibleSpace();

                        if (GUILayout.Button("Create Skeleton"))
                        {
                            GameObject skeletonPrefab = AvatarCreatorUtil.CreateSkeleton(skeletonCreatorData);
                            if (skeletonPrefab == null)
                            {
                                EditorUtility.DisplayDialog("Error", "Create Failed.\n Please view the details from the console!!!", "OK");
                            }
                            else
                            {
                                SelectionUtility.PingObject(skeletonPrefab);
                            }
                        }
                    }
                }
                EditorGUILayout.EndVertical();
            }
            GUILayout.EndArea();
        }

        private void DrawParts(Rect rect)
        {
            GUILayout.BeginArea(rect);
            {
                EditorGUILayout.BeginVertical();
                {
                    EGUILayout.DrawBoxHeader("Part Data", EGUIStyles.BoxedHeaderCenterStyle,GUILayout.ExpandWidth(true));
                    if (currentCreatorData != null && partOutputDataDrawer!=null)
                    {
                        partOutputDataDrawer.OnGUILayout();


                        GUILayout.FlexibleSpace();

                        if (GUILayout.Button("Create Parts"))
                        {
                            PartOutputData partOutputData = currentCreatorData.partOutputData;
                            foreach (var data in partOutputData.partDatas)
                            {
                                if (!CreatePart(data))
                                {
                                    break;
                                }
                            }
                        }

                        if (GUILayout.Button("Show Preview"))
                        {
                            ShowPreview();
                        }
                    }
                }
                EditorGUILayout.EndVertical();
            }
            GUILayout.EndArea();
        }

        private bool CreatePart(PartCreatorData data)
        {
            var partData = AvatarCreatorUtil.CreatePart(currentCreatorData.partOutputData.outputFolder, data);
            if (partData == null)
            {
                EditorUtility.DisplayDialog("Error", "Create Failed.\n Please view the details from the console!!!", "OK");
                return false;
            }
            else
            {
                SelectionUtility.PingObject(partData);
                return true;
            }
        }

        private void ShowPreview()
        {

        }
    }
}
