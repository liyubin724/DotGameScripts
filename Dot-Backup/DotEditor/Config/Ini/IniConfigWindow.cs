﻿using Dot.Config.Ini;
using Dot.Generic;
using DotEditor.GUIExtension;
using DotEditor.Utilities;
using ReflectionMagic;
using UnityEditor;
using UnityEngine;

namespace DotEditor.Config.Ini
{
    public class IniConfigWindow :EditorWindow
    {
        [MenuItem("Game/Ini Window")]
        private static IniConfigWindow ShowWin()
        {
            IniConfigWindow win = EditorWindow.GetWindow<IniConfigWindow>();
            win.titleContent = Contents.WinTitleContent;
            win.Show();
            return win;
        }

        private IniConfig iniConfig = null;
        private string assetPath = null;

        private void OnGUI()
        {
            DrawToolbar();

            if (iniConfig != null)
            {
                dynamic config = iniConfig.AsDynamic();
                ListDictionary<string, IniGroup> groups = config.groups;

                for(int i =0;i<groups.Count;++i)
                {
                    DrawGroup(groups[i]);
                }
            }
        }

        private ToolbarSearchField searchField = null;
        private string searchText = string.Empty;
        private void DrawToolbar()
        {
            EditorGUILayout.BeginHorizontal(EditorStyles.toolbar, GUILayout.ExpandWidth(true));
            {
                if (EGUILayout.ToolbarButton(Contents.OpenStr))
                {
                    DoOpen();
                }

                if (EGUILayout.ToolbarButton(Contents.NewStr))
                {
                    DoCreate();
                }

                EditorGUI.BeginDisabledGroup(iniConfig == null);
                {
                    if (EGUILayout.ToolbarButton(Contents.SaveStr))
                    {
                        DoSave();
                    }
                }
                EditorGUI.EndDisabledGroup();

                UnityEngine.GUILayout.FlexibleSpace();

                EditorGUI.BeginDisabledGroup(iniConfig == null);
                {
                    if (EGUILayout.ToolbarButton(Contents.AddGroupContent))
                    {
                        Vector2 size = new Vector2(300, 150);
                        Rect rect = new Rect(position.position + 0.5f * position.size - size * 0.5f, size);

                        CreateGroupPopupContent.ShowWin(iniConfig,rect);
                    }
                    if (searchField == null)
                    {
                        searchField = new ToolbarSearchField((text) =>
                        {
                            searchText = text == null ? "" : text.ToLower();
                        }, null);
                    }
                    searchField.OnGUILayout();
                }
                EditorGUI.EndDisabledGroup();
            }
            EditorGUILayout.EndHorizontal();
        }

        private void DoOpen()
        {
            string filePath = EditorUtility.OpenFilePanel(Contents.OpenStr, Application.dataPath, "txt");
            if (!string.IsNullOrEmpty(filePath))
            {
                iniConfig = null;

                assetPath = PathUtility.GetAssetPath(filePath);
                if (string.IsNullOrEmpty(assetPath))
                {
                    return;
                }

                TextAsset ta = AssetDatabase.LoadAssetAtPath<TextAsset>(assetPath);
                if (ta != null && !string.IsNullOrEmpty(ta.text))
                {
                    iniConfig = new IniConfig();
                    iniConfig.IsReadonly = false;
                    iniConfig.ParseText(ta.text);

                    Repaint();
                }
            }
        }

        private void DoSave()
        {
            if(iniConfig == null)
            {
                return;
            }

            if(string.IsNullOrEmpty(assetPath))
            {
                string filePath = EditorUtility.SaveFilePanel(Contents.SaveStr, Application.dataPath, "ini_config", "txt");
                if(!string.IsNullOrEmpty(filePath))
                {
                    assetPath = PathUtility.GetAssetPath(filePath);
                }
            }

            iniConfig.Save(PathUtility.GetDiskPath(assetPath));
            AssetDatabase.ImportAsset(assetPath);
        }

        private void DoCreate()
        {
            assetPath = null;
            iniConfig = new IniConfig();
            iniConfig.IsReadonly = false;
        }

        private void DrawGroup(IniGroup group)
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            {
                EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
                {
                    EditorGUILayout.LabelField(new GUIContent(group.Name, group.Comment));

                    if(GUILayout.Button(Contents.DeleteGroupContent, EditorStyles.toolbarButton, UnityEngine.GUILayout.Width(80)))
                    {
                        iniConfig.DeleteGroup(group.Name);
                        GUIUtility.ExitGUI();
                        Repaint();
                    }

                    if (GUILayout.Button(Contents.AddDataContent, EditorStyles.toolbarButton, UnityEngine.GUILayout.Width(80)))
                    {
                        //Vector2 pos = UnityEngine.GUIUtility.GUIToScreenPoint(Input.mousePosition);
                        Vector2 size = new Vector2(300, 200);
                        Rect rect = new Rect(position.position + 0.5f* position.size - size * 0.5f, size);
                        CreateDataPopupContent.ShowWin(group, rect);
                    }
                }
                EditorGUILayout.EndHorizontal();

                ListDictionary<string, IniData> datas = group.AsDynamic().datas;
                for(int i =0;i<datas.Count;++i)
                {
                    IniData data = datas[i];

                    if(string.IsNullOrEmpty(searchText) || data.Key.ToLower().IndexOf(searchText)>=0)
                    {
                        DrawData(group, data);
                    }
                }
            }
            EditorGUILayout.EndVertical();
        }

        private void DrawData(IniGroup group,IniData data)
        {
            string value = data.Value;
            EditorGUILayout.BeginHorizontal();
            {
                if(data.OptionValues!=null && data.OptionValues.Length>0)
                {
                    value = EGUILayout.StringPopup(new GUIContent(data.Key,data.Comment), data.Value, data.OptionValues);
                }else
                {
                    value = EditorGUILayout.TextField(new GUIContent(data.Key, data.Comment), data.Value);
                }

                if(GUILayout.Button(Contents.DeleteContent, GUILayout.Width(20)))
                {
                    group.RemoveData(data.Key);
                    GUIUtility.ExitGUI();
                    Repaint();
                }
            }
            EditorGUILayout.EndHorizontal();
            if(data.Value!=value)
            {
                data.Value = value;
            }
        }

        static class Contents
        {
            internal static GUIContent WinTitleContent = new GUIContent("Ini Config");
            internal static GUIContent ChangedWinTitleContent = new GUIContent("Ini Config *");

            internal static string SaveStr = "Save";
            internal static string SaveForChangedMessageStr = "The config is changed,do you want to save it?";
            internal static string OKStr = "OK";
            internal static string CancelStr = "Cancel";

            internal static string OpenStr = "Open";

            internal static string NewStr = "New";

            internal static GUIContent AddGroupContent = new GUIContent("Add Group", "Add a new group");
            internal static GUIContent DeleteGroupContent = new GUIContent("Delete Group", "Delete the group");
            internal static GUIContent AddDataContent = new GUIContent("Add Data", "Add a new data");
            internal static GUIContent DeleteContent = new GUIContent("-");
        }
    }
}
