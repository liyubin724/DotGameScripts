using DotEditor.Core.EGUI;
using DotEditor.Util;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static DotEditor.Entity.Avatar.AvatarCreatorData;

namespace DotEditor.Entity.Avatar
{
    public class AvatarSkeletonCreatorDrawer
    {
        private static int DATA_LIST_ITEM_WIDTH = 120;
        private static int DATA_LIST_ITEM_HEIGHT = 40;

        private Action repaintCallback = null;

        public AvatarSkeletonCreatorDrawer(Action repaint)
        {
            repaintCallback = repaint;
        }

        private List<AvatarSkeletonCreatorData> dataList = null;
        internal void SetData(List<AvatarSkeletonCreatorData> dataList)
        {
            this.dataList = dataList;
        }

        internal void OnGUI()
        {
            if(dataList!=null && isDelete && selectedData!=null)
            {
                int index = dataList.IndexOf(selectedData);
                dataList.RemoveAt(index);

                if(index>= dataList.Count)
                {
                    index = dataList.Count - 1;
                }
                if(index>=0)
                {
                    OnDataSelected(dataList[index]);
                }
                isDelete = false;
            }

            EditorGUILayout.BeginVertical(GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));
            {
                DrawToolbar();
                EditorGUILayout.BeginHorizontal(EditorStyles.helpBox, GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));
                {
                    EditorGUILayout.BeginVertical(EditorStyles.helpBox, GUILayout.Width(DATA_LIST_ITEM_WIDTH), GUILayout.ExpandHeight(true));
                    {
                        DrawDataList();
                    }
                    EditorGUILayout.EndVertical();
                    EditorGUILayout.BeginVertical(EditorStyles.helpBox, GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));
                    {
                        if (selectedData != null)
                        {
                            DrawSkeletonData();

                            if (GUILayout.Button(Contents.CreateSkeletonBtnContent, GUILayout.Height(40)))
                            {
                                ExportSkeleton(selectedData);
                            }
                        }
                    }
                    EditorGUILayout.EndVertical();
                }
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndVertical();
        }

        private void DrawToolbar()
        {
            GUILayout.BeginHorizontal("toolbar", GUILayout.ExpandWidth(true));
            {
                if (GUILayout.Button(Contents.NewBtnContent, EditorStyles.toolbarButton, GUILayout.Width(60)))
                {
                    if(dataList!=null)
                    {
                        AvatarSkeletonCreatorData data = new AvatarSkeletonCreatorData();
                        data.dataName = "New Data";
                        dataList.Add(data);

                        OnDataSelected(data);
                    }
                }
                if (GUILayout.Button(Contents.DeleteBtnContent, EditorStyles.toolbarButton, GUILayout.Width(60)))
                {
                    isDelete = true;
                }
                if (GUILayout.Button(Contents.ExportBtnContent, EditorStyles.toolbarButton, GUILayout.Width(60)))
                {
                    if(selectedData!=null)
                    {
                        ExportSkeleton(selectedData);
                    }
                }
                GUILayout.FlexibleSpace();
                if (GUILayout.Button(Contents.ExportAllBtnContent, EditorStyles.toolbarButton, GUILayout.Width(60)))
                {
                    foreach(var d in dataList)
                    {
                        ExportSkeleton(d);
                    }
                }
            }
            GUILayout.EndHorizontal();
        }

        private AvatarSkeletonCreatorData selectedData = null;
        private bool isDelete = false;
        private void DrawDataList()
        {
            if(dataList!=null)
            {
                foreach(var data in dataList)
                {
                    Color color = GUI.backgroundColor;
                    if (selectedData != null && data == selectedData)
                    {
                        GUI.backgroundColor = Color.blue;
                    }
                    if (GUILayout.Button(data.dataName, GUILayout.ExpandWidth(true), GUILayout.Height(DATA_LIST_ITEM_HEIGHT)))
                    {
                        OnDataSelected(data);
                    }
                    GUI.backgroundColor = color;
                }
            }
        }

        private void OnDataSelected(AvatarSkeletonCreatorData data)
        {
            selectedData = data;

            repaintCallback();
        }
        
        private void DrawSkeletonData()
        {
            selectedData.dataName = EditorGUILayout.TextField(Contents.DataNameContent, selectedData.dataName);
            selectedData.isEnable = EditorGUILayout.Toggle(Contents.IsEnableContent, selectedData.isEnable);
            if(string.IsNullOrEmpty(selectedData.savedDir))
            {
                EditorGUILayout.HelpBox(Contents.SavedDirEmptyStr, MessageType.Error);
            }else if(!AssetDatabase.IsValidFolder(selectedData.savedDir))
            {
                EditorGUILayout.HelpBox(Contents.SavedDirNotExitStr, MessageType.Error);
            }
            selectedData.savedDir = EditorGUILayoutUtil.DrawAssetFolderSelection(Contents.SavedDirStr, selectedData.savedDir, false);
            if(selectedData.fbxPrefab == null)
            {
                EditorGUILayout.HelpBox(Contents.FBXPrefabNullStr, MessageType.Error);
            }
            else
            {
                PrefabAssetType assetType = PrefabUtility.GetPrefabAssetType(selectedData.fbxPrefab);
                if(assetType != PrefabAssetType.Model)
                {
                    EditorGUILayout.HelpBox(Contents.FBXPrefabNotModelStr, MessageType.Error);
                }
            }
            selectedData.fbxPrefab = (GameObject)EditorGUILayout.ObjectField(Contents.FBXPrefabContent, selectedData.fbxPrefab, typeof(GameObject), false);
        }

        private void ExportSkeleton(AvatarSkeletonCreatorData data)
        {
            if(string.IsNullOrEmpty(data.savedDir))
            {
                EditorUtility.DisplayDialog(Contents.ErrorTitleStr, Contents.SavedDirEmptyStr, Contents.OKBtnStr);
            }else if(!AssetDatabase.IsValidFolder(data.savedDir))
            {
                EditorUtility.DisplayDialog(Contents.ErrorTitleStr, Contents.SavedDirNotExitStr, Contents.OKBtnStr);
            }else
            {
                GameObject prefab = AvatarCreatorUtil.CreateSkeleton(data);
                SelectionUtil.ActiveObject(prefab);
            }
        }


        class Contents
        {
            internal static GUIContent CreateSkeletonBtnContent = new GUIContent("Create Skeleton");

            internal static GUIContent DataNameContent = new GUIContent("Data Name");
            internal static string SavedDirStr = "Saved Dir";
            internal static GUIContent IsEnableContent = new GUIContent("Is Enable");
            internal static GUIContent FBXPrefabContent = new GUIContent("FBX Prefab");

            internal static GUIContent NewBtnContent = new GUIContent("New");
            internal static GUIContent DeleteBtnContent = new GUIContent("Delete");
            internal static GUIContent ExportBtnContent = new GUIContent("Export");
            internal static GUIContent ExportAllBtnContent = new GUIContent("Export All");

            internal static string OKBtnStr = "OK";
            internal static string ErrorTitleStr = "Error";

            internal static string SavedDirEmptyStr = "The folder is empty.please select a folder in which the prefab will be saved";
            internal static string SavedDirNotExitStr = "The folder is not exist";

            internal static string FBXPrefabNullStr = "The fbxPrefab should not be null";
            internal static string FBXPrefabNotModelStr = "The prefab is not a model";
        }
    }
}
