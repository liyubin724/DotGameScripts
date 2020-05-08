using Dot.Config.Ini;
using DotEditor.Core;
using DotEditor.Core.Utilities;
using ReflectionMagic;
using System.Collections.Generic;
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
        private string configAssetPath = null;
        public string ConfigAssetPath
        {
            get 
            {
                return configAssetPath;
            }
            set
            {
                if(configAssetPath!=value)
                {
                    configAssetPath = value;
                    LoadIniConfig();
                }
            }
        }

        private void LoadIniConfig()
        {
            iniConfig = null;

            if(string.IsNullOrEmpty(configAssetPath))
            {
                return;
            }
            TextAsset ta = AssetDatabase.LoadAssetAtPath<TextAsset>(configAssetPath);
            if(ta!=null && !string.IsNullOrEmpty(ta.text))
            {
                iniConfig = new IniConfig(ta.text, false);
                Repaint();
            }
        }

        private void OpenIniConfig()
        {
            string filePath = EditorUtility.OpenFilePanel(Contents.OpenStr, Application.dataPath, "txt");
            if(!string.IsNullOrEmpty(filePath))
            {
                ConfigAssetPath = PathUtility.GetAssetPath(filePath);
            }
        }

        private void SaveIniConfig()
        {
            if(iniConfig == null)
            {
                return;
            }

            if(string.IsNullOrEmpty(ConfigAssetPath))
            {
                string filePath = EditorUtility.SaveFilePanel(Contents.SaveStr, Application.dataPath, "ini_config", "txt");
                if(!string.IsNullOrEmpty(filePath))
                {
                    configAssetPath = PathUtility.GetAssetPath(filePath);
                }
            }

            iniConfig.Save(PathUtility.GetDiskPath(ConfigAssetPath));
            AssetDatabase.ImportAsset(ConfigAssetPath);
        }

        private void NewIniConfig()
        {
            configAssetPath = null;
            iniConfig = new IniConfig();
        }

        private DeleteData deleteData = null;
        private void OnGUI()
        {
            DrawToolbar();
            if(iniConfig!=null)
            {
                dynamic config = iniConfig.AsDynamic();
                Dictionary<string, IniGroup> groupDic = config.groupDic;

                if(deleteData!=null)
                {
                    if(string.IsNullOrEmpty(deleteData.dataKey))
                    {
                        if(groupDic.ContainsKey(deleteData.groupName))
                        {
                            groupDic.Remove(deleteData.groupName);
                        }
                    }else
                    {
                        if(groupDic.TryGetValue(deleteData.groupName,out IniGroup g))
                        {
                            g.DeleteData(deleteData.dataKey);
                        }
                    }

                    deleteData = null;
                }

                foreach(var kvp in groupDic)
                {
                    DrawGroup(kvp.Value);
                }
            }
        }

        private EGUIToolbarSearchField searchField = null;
        private string searchText = string.Empty;
        private void DrawToolbar()
        {
            EditorGUILayout.BeginHorizontal(EditorStyles.toolbar, UnityEngine.GUILayout.ExpandWidth(true));
            {
                if(UnityEngine.GUILayout.Button(Contents.OpenStr, EditorStyles.toolbarButton, UnityEngine.GUILayout.Width(40)))
                {
                    OpenIniConfig();
                }
                
                if (UnityEngine.GUILayout.Button(Contents.NewStr, EditorStyles.toolbarButton, UnityEngine.GUILayout.Width(40)))
                {
                    NewIniConfig();
                }
                if(iniConfig!=null)
                {
                    if (UnityEngine.GUILayout.Button(Contents.SaveStr, EditorStyles.toolbarButton, UnityEngine.GUILayout.Width(40)))
                    {
                        SaveIniConfig();
                    }
                }

                UnityEngine.GUILayout.FlexibleSpace();
                if(iniConfig!=null)
                {
                    if (UnityEngine.GUILayout.Button(Contents.AddGroupContent, EditorStyles.toolbarButton, UnityEngine.GUILayout.Width(80)))
                    {
                        Vector2 size = new Vector2(300, 150);
                        Rect rect = new Rect(position.position + 0.5f * position.size - size * 0.5f, size);
                        CreateIniGroupPopupWindow.ShowWin(iniConfig, (object name)=>
                        {
                        }, rect);
                    }
                }
                if(searchField == null)
                {
                    searchField = new EGUIToolbarSearchField((text) =>
                    {
                        searchText = text==null?"":text.ToLower();
                    },null);
                }
                searchField.OnGUILayout();
            }
            EditorGUILayout.EndHorizontal();
        }

        private void DrawGroup(IniGroup group)
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            {
                EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
                {
                    EditorGUILayout.LabelField(new GUIContent(group.Name, group.Comment));

                    if (UnityEngine.GUILayout.Button(Contents.DeleteGroupContent, EditorStyles.toolbarButton, UnityEngine.GUILayout.Width(80)))
                    {
                        deleteData = new DeleteData() { groupName = group.Name };
                    }

                    if (UnityEngine.GUILayout.Button(Contents.AddDataContent, EditorStyles.toolbarButton, UnityEngine.GUILayout.Width(80)))
                    {
                        Vector2 pos = UnityEngine.GUIUtility.GUIToScreenPoint(Input.mousePosition);
                        Vector2 size = new Vector2(300, 200);
                        Rect rect = new Rect(position.position + 0.5f* position.size - size * 0.5f, size);
                        CreateIniDataPopupWindow.ShowWin(group, (object groupName, object dataKey)=>
                        {
                        }, rect);
                    }
                }
                EditorGUILayout.EndHorizontal();

                foreach(var dKVP in group.dataDic)
                {
                    if(string.IsNullOrEmpty(searchText))
                    {
                        DrawData(group, dKVP.Value);
                    }else if(dKVP.Key.ToLower().IndexOf(searchText) >= 0)
                    {
                        DrawData(group, dKVP.Value);
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
                    value = DotEditorGUILayout.StringPopup(new GUIContent(data.Key,data.Comment), data.Value, data.OptionValues);
                }else
                {
                    value = EditorGUILayout.TextField(new GUIContent(data.Key, data.Comment), data.Value);
                }

                if(UnityEngine.GUILayout.Button(Contents.DeleteContent, UnityEngine.GUILayout.Width(20)))
                {
                    deleteData = new DeleteData() { groupName = group.Name, dataKey = data.Key };
                }
            }
            EditorGUILayout.EndHorizontal();
            if(data.Value!=value)
            {
                data.Value = value;
            }
        }

        class DeleteData
        {
            public string groupName;
            public string dataKey;
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
