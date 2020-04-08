using DotEditor.Core.Utilities;
using System;
using UnityEditor;
using UnityEngine;
using UnityObject = UnityEngine.Object;

namespace DotEditor.EGUI
{
    public static class DEGUILayout
    {
        public static void DrawScript(UnityObject target)
        {
            Type targetType = target.GetType();
            if(typeof(MonoBehaviour).IsAssignableFrom(targetType))
            {
                EditorGUILayout.ObjectField("Script", MonoScript.FromMonoBehaviour((MonoBehaviour)target), typeof(MonoScript), false);
            }else if(typeof(ScriptableObject).IsAssignableFrom(targetType))
            {
                EditorGUILayout.ObjectField("Script", MonoScript.FromScriptableObject((ScriptableObject)target), typeof(MonoScript), false);
            }else
            {
                EditorGUILayout.LabelField("Script", targetType.FullName);
            }
        }

        public static T DrawPopup<T>(string label, string[] contents, T[] values, T selectedValue)
        {
            int index = Array.IndexOf(values, selectedValue);
            if (index < 0) index = 0;
            int newIndex = EditorGUILayout.Popup(label, index, contents);

            return values[newIndex];
        }

        public static void DrawAssetFolderSelection(SerializedProperty property, bool isReadonly = true)
        {
            EditorGUILayout.BeginHorizontal();
            {
                EditorGUI.BeginDisabledGroup(isReadonly);
                {
                    EditorGUILayout.PropertyField(property);
                }
                EditorGUI.EndDisabledGroup();

                if (GUILayout.Button(new GUIContent(DEGUIResources.FolderIcon), GUILayout.Width(20), GUILayout.Height(20)))
                {
                    string folderPath = EditorUtility.OpenFolderPanel("folder", property.stringValue, "");
                    if (!string.IsNullOrEmpty(folderPath))
                    {
                        property.stringValue = PathUtility.GetAssetPath(folderPath);
                    }
                }
                if (GUILayout.Button("\u2716", GUILayout.Width(20), GUILayout.Height(20)))
                {
                    property.stringValue = "";
                }
            }
            EditorGUILayout.EndHorizontal();
        }

        public static string DrawAssetFolderSelection(string label, string assetFolder, bool isReadonly = true)
        {
            string folder = assetFolder;
            EditorGUILayout.BeginHorizontal();
            {
                EditorGUI.BeginDisabledGroup(isReadonly);
                {
                    folder = EditorGUILayout.TextField(label, assetFolder);
                }
                EditorGUI.EndDisabledGroup();

                if (GUILayout.Button(new GUIContent(DEGUIResources.FolderIcon), GUILayout.Width(20), GUILayout.Height(20)))
                {
                    string folderPath = EditorUtility.OpenFolderPanel("folder", folder, "");
                    if (!string.IsNullOrEmpty(folderPath))
                    {
                        folder = PathUtility.GetAssetPath(folderPath);
                    }
                }
                if (GUILayout.Button("\u2716", GUILayout.Width(20), GUILayout.Height(20)))
                {
                    folder = "";
                }
            }
            EditorGUILayout.EndHorizontal();
            return folder;
        }

        public static string DrawDiskFolderSelection(string label, string diskFolder, bool isReadonly = true)
        {
            EditorGUILayout.BeginHorizontal();
            {
                EditorGUI.BeginDisabledGroup(isReadonly);
                {
                    EditorGUILayout.TextField(label, diskFolder);
                }
                EditorGUI.EndDisabledGroup();

                if (GUILayout.Button(new GUIContent(DEGUIResources.FolderIcon), GUILayout.Width(20), GUILayout.Height(20)))
                {
                    diskFolder = EditorUtility.OpenFolderPanel("folder", diskFolder, "");
                }
                if (GUILayout.Button("\u2716", GUILayout.Width(20), GUILayout.Height(20)))
                {
                    diskFolder = "";
                }
            }
            EditorGUILayout.EndHorizontal();

            return diskFolder;
        }

        public static string StringPopup(string label, string selected, string[] optionValues)
        {
            if (optionValues == null)
            {
                optionValues = new string[0];
            }

            int selectedIndex = Array.IndexOf(optionValues, selected);

            int newSelectedIndex = EditorGUILayout.Popup(label, selectedIndex, optionValues);
            if (newSelectedIndex >= 0 && newSelectedIndex < optionValues.Length)
            {
                return optionValues[newSelectedIndex];
            }
            return selected;
        }

        public static string StringPopup(GUIContent label, string selected, string[] optionValues)
        {
            if (optionValues == null)
            {
                optionValues = new string[0];
            }

            int selectedIndex = Array.IndexOf(optionValues, selected);

            int newSelectedIndex = EditorGUILayout.Popup(label, selectedIndex, optionValues);
            if (newSelectedIndex >= 0 && newSelectedIndex < optionValues.Length)
            {
                return optionValues[newSelectedIndex];
            }
            return selected;
        }
    }
}
