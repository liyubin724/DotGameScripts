using Dot.Entity.Avatar;
using DotEditor.Core.EGUI;
using DotEditor.Util;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using static DotEditor.Entity.Avatar.AvatarCreatorData;

namespace DotEditor.Entity.Avatar
{
    public class AvatarPartCreatorDrawer
    {
        private static int DATA_LIST_ITEM_WIDTH = 120;
        private static int DATA_LIST_ITEM_HEIGHT = 40;

        private Action repaintCallback = null;

        public AvatarPartCreatorDrawer(Action repaint)
        {
            repaintCallback = repaint;
        }

        private List<AvatarPartCreatorData> dataList = null;
        internal void SetData(List<AvatarPartCreatorData> dataList)
        {
            this.dataList = dataList;
        }

        private Vector2 prefabPartScrollPos = Vector2.zero;
        private Vector2 rendererPartScrollPos = Vector2.zero;

        internal void OnGUI()
        {
            if (dataList != null && isDelete && selectedData != null)
            {
                int index = dataList.IndexOf(selectedData);
                dataList.RemoveAt(index);

                if (index >= dataList.Count)
                {
                    index = dataList.Count - 1;
                }
                if (index >= 0)
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
                            DrawPartData();

                            if(GUILayout.Button(Contents.PartCreatorBtnContent,GUILayout.Height(40)))
                            {
                                ExportPartData(selectedData);
                            }

                            EditorGUILayout.BeginHorizontal();
                            {
                                prefabPartScrollPos = EditorGUILayout.BeginScrollView(prefabPartScrollPos);
                                {
                                    if(prefabPartRList!=null)
                                    {
                                        prefabPartRList.DoLayoutList();
                                    }
                                }
                                EditorGUILayout.EndScrollView();

                                rendererPartScrollPos = EditorGUILayout.BeginScrollView(rendererPartScrollPos);
                                {
                                    if(rendererPartRList!=null)
                                    {
                                        rendererPartRList.DoLayoutList();
                                    }
                                }
                                EditorGUILayout.EndScrollView();
                            }
                            EditorGUILayout.EndHorizontal();
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
                    if (dataList != null)
                    {
                        AvatarPartCreatorData data = new AvatarPartCreatorData();
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
                        ExportPartData(selectedData);
                    }
                }
                GUILayout.FlexibleSpace();
                if (GUILayout.Button(Contents.ExportAllBtnContent, EditorStyles.toolbarButton, GUILayout.Width(60)))
                {
                    foreach(var d in dataList)
                    {
                        ExportPartData(d);
                    }
                }
            }
            GUILayout.EndHorizontal();
        }

        private AvatarPartCreatorData selectedData = null;
        private bool isDelete = false;
        private void DrawDataList()
        {
            if (dataList != null)
            {
                foreach (var data in dataList)
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

        private ReorderableList prefabPartRList = null;
        private ReorderableList rendererPartRList = null;
        private void OnDataSelected(AvatarPartCreatorData data)
        {
            selectedData = data;

            prefabPartRList = new ReorderableList(data.prefabPartDatas, typeof(AvatarPrefabCreatorData), true, true, true,true);
            prefabPartRList.elementHeight = EditorGUIUtility.singleLineHeight * 6;
            prefabPartRList.drawHeaderCallback = (rect) =>
            {
                EditorGUI.LabelField(rect, Contents.PrefabCreatorListContent);
            };
            prefabPartRList.drawElementCallback = (rect, index, isActive, isFocused) =>
            {
                AvatarPrefabCreatorData partData = data.prefabPartDatas[index];

                Rect drawRect = rect;
                drawRect.height = EditorGUIUtility.singleLineHeight;
                
                partData.dataName = EditorGUI.TextField(drawRect,Contents.DataNameContent, partData.dataName);
                drawRect.y += drawRect.height;
                partData.isEnable = EditorGUI.Toggle(drawRect,Contents.IsEnableContent, partData.isEnable);
                drawRect.y += drawRect.height;
                partData.savedDir = EditorGUIUtil.DrawAssetFolderSelection(drawRect, Contents.SavedDirStr, partData.savedDir, false);
                drawRect.y += drawRect.height;
                partData.bindNodeName = EditorGUI.TextField(drawRect, Contents.BindNodeNameContent, partData.bindNodeName);
                drawRect.y += drawRect.height;
                partData.bindPrefab = (GameObject)EditorGUI.ObjectField(drawRect, Contents.BindPrefabContent, partData.bindPrefab,typeof(GameObject),false);
            };
            prefabPartRList.onAddCallback = (list) =>
            {
                AvatarPrefabCreatorData newData = new AvatarPrefabCreatorData();
                newData.dataName = "New Data";
                list.list.Add(newData);
            };

            rendererPartRList = new ReorderableList(data.rendererPartDatas, typeof(AvatarRendererCreatorData), true, true, true, true);
            rendererPartRList.elementHeight = EditorGUIUtility.singleLineHeight * 5;
            rendererPartRList.drawHeaderCallback = (rect) =>
            {
                EditorGUI.LabelField(rect, Contents.RendererCreatorListContent);
            };
            rendererPartRList.drawElementCallback = (rect, index, isActive, isFocused) =>
            {
                AvatarRendererCreatorData partData = data.rendererPartDatas[index];

                Rect drawRect = rect;
                drawRect.height = EditorGUIUtility.singleLineHeight;

                partData.dataName = EditorGUI.TextField(drawRect, Contents.DataNameContent, partData.dataName);
                drawRect.y += drawRect.height;
                partData.isEnable = EditorGUI.Toggle(drawRect, Contents.IsEnableContent, partData.isEnable);
                drawRect.y += drawRect.height;
                partData.savedDir = EditorGUIUtil.DrawAssetFolderSelection(drawRect, Contents.SavedDirStr, partData.savedDir, false);
                drawRect.y += drawRect.height;
                partData.fbxPrefab = (GameObject)EditorGUI.ObjectField(drawRect, Contents.BindPrefabContent, partData.fbxPrefab, typeof(GameObject), false);
            };
            rendererPartRList.onAddCallback = (list) =>
            {
                AvatarRendererCreatorData newData = new AvatarRendererCreatorData();
                newData.dataName = "New Data";
                list.list.Add(newData);
            };

            repaintCallback();
        }

        private void DrawPartData()
        {
            selectedData.dataName = EditorGUILayout.TextField(Contents.DataNameContent, selectedData.dataName);
            selectedData.isEnable = EditorGUILayout.Toggle(Contents.IsEnableContent, selectedData.isEnable);
            if (string.IsNullOrEmpty(selectedData.savedDir))
            {
                EditorGUILayout.HelpBox(Contents.SavedDirEmptyStr, MessageType.Error);
            }
            else if (!AssetDatabase.IsValidFolder(selectedData.savedDir))
            {
                EditorGUILayout.HelpBox(Contents.SavedDirNotExitStr, MessageType.Error);
            }
            selectedData.savedDir = EditorGUILayoutUtil.DrawAssetFolderSelection(Contents.SavedDirStr, selectedData.savedDir, false);
            selectedData.partType = (AvatarPartType)EditorGUILayout.EnumPopup(Contents.PartTypeContent, selectedData.partType);
        }

        private void ExportPartData(AvatarPartCreatorData data)
        {
            if (string.IsNullOrEmpty(data.savedDir) || !AssetDatabase.IsValidFolder(data.savedDir))
            {
                EditorUtility.DisplayDialog(Contents.ErrorTitleStr, Contents.ErrorForSavedDir, Contents.OKStr);
            }
            else if (data.partType <= AvatarPartType.None || data.partType >= AvatarPartType.Max)
            {
                EditorUtility.DisplayDialog(Contents.ErrorTitleStr, Contents.ErrorForPartType, Contents.OKStr);
            }
            else
            {
                AvatarPartData partData = AvatarCreatorUtil.CreatePart(data);
                SelectionUtil.ActiveObject(partData);
            }
        }

        class Contents
        {
            internal static GUIContent CreateSkeletonBtnContent = new GUIContent("Create Skeleton");

            internal static GUIContent DataNameContent = new GUIContent("Data Name");
            internal static string SavedDirStr = "Saved Dir";
            internal static GUIContent IsEnableContent = new GUIContent("Is Enable");
            internal static GUIContent PartTypeContent = new GUIContent("Part Type");

            internal static GUIContent PrefabCreatorListContent = new GUIContent("Prefab Creator");
            internal static GUIContent RendererCreatorListContent = new GUIContent("Renderer Creator");

            internal static GUIContent BindNodeNameContent = new GUIContent("Bind Node Name");
            internal static GUIContent BindPrefabContent = new GUIContent("BindPrefab");

            internal static GUIContent NewBtnContent = new GUIContent("New");
            internal static GUIContent DeleteBtnContent = new GUIContent("Delete");
            internal static GUIContent ExportBtnContent = new GUIContent("Export");
            internal static GUIContent ExportAllBtnContent = new GUIContent("Export All");

            internal static GUIContent PartCreatorBtnContent = new GUIContent("Create Part");

            internal static string OKStr = "OK";
            internal static string ErrorTitleStr = "Error";
            internal static string ErrorForSavedDir = "The folder is not correct,please fixed it at first";
            internal static string ErrorForPartType = "The partType is Error.";

            internal static string SavedDirEmptyStr = "The folder is empty.please select a folder in which the prefab will be saved";
            internal static string SavedDirNotExitStr = "The folder is not exist";

            internal static string FBXPrefabNullStr = "The fbxPrefab should not be null";
            internal static string FBXPrefabNotModelStr = "The prefab is not a model";
        }
    }
}
