using DotEditor.Utilities;
using System;
using UnityEditor;
using UnityEngine;
using UnityObject = UnityEngine.Object;

namespace DotEditor.GUIExtension
{
    public static class EGUILayout
    {
        #region DrawLine
        public static void DrawHorizontalLine()
        {
            DrawHorizontalLine(EGUIResources.gray);
        }

        public static void DrawHorizontalLine(Color color,float thickness = 0.75f, float padding = 6.0f)
        {
            Rect rect = EditorGUILayout.GetControlRect(UnityEngine.GUILayout.Height(padding + thickness), UnityEngine.GUILayout.ExpandWidth(true));
            EGUI.DrawHorizontalLine(rect, color,thickness, padding);
        }

        public static void DrawVerticalLine()
        {
            DrawVerticalLine(EGUIResources.gray);
        }

        public static void DrawVerticalLine(Color color,float thickness = 0.75f, float padding = 6.0f )
        {
            Rect rect = EditorGUILayout.GetControlRect(UnityEngine.GUILayout.Width(padding + thickness), UnityEngine.GUILayout.ExpandHeight(true));
            EGUI.DrawVerticalLine(rect, color,thickness, padding);
        }

        #endregion

        public static void DrawAssetPreview(UnityObject uObj,float width = 64,float height = 64)
        {
            var previewTexture = AssetPreview.GetAssetPreview(uObj);
            if(previewTexture!=null)
            {
                width = Mathf.Clamp(width, 0, previewTexture.width);
                height = Mathf.Clamp(height, 0, previewTexture.height);
                var previewOptions = new GUILayoutOption[]
                {
                    UnityEngine.GUILayout.MaxWidth(width),
                    UnityEngine.GUILayout.MaxHeight(height),
                };
                Rect rect = EditorGUILayout.GetControlRect(true, height, previewOptions);
                EditorGUI.LabelField(rect, GUIContent.none, EGUIStyles.GetTextureStyle(previewTexture));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="label"></param>
        /// <param name="options"></param>
        public static void DrawBoxHeader(string label,params GUILayoutOption[] options)
        {
            EditorGUILayout.LabelField(label, EGUIStyles.BoxedHeaderStyle,options);
        }

        public static void DrawScript(UnityObject target)
        {
            Type targetType = target.GetType();
            EditorGUI.BeginDisabledGroup(true);
            {
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
            EditorGUI.EndDisabledGroup();
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

                if (UnityEngine.GUILayout.Button(new GUIContent(EGUIResources.DefaultFolderIcon), UnityEngine.GUILayout.Width(20), UnityEngine.GUILayout.Height(20)))
                {
                    string folderPath = EditorUtility.OpenFolderPanel("folder", property.stringValue, "");
                    if (!string.IsNullOrEmpty(folderPath))
                    {
                        property.stringValue = PathUtility.GetAssetPath(folderPath);
                    }
                }
                if (UnityEngine.GUILayout.Button("\u2716", UnityEngine.GUILayout.Width(20), UnityEngine.GUILayout.Height(20)))
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

                if (UnityEngine.GUILayout.Button(new GUIContent(EGUIResources.DefaultFolderIcon), UnityEngine.GUILayout.Width(20), UnityEngine.GUILayout.Height(20)))
                {
                    string folderPath = EditorUtility.OpenFolderPanel("folder", folder, "");
                    if (!string.IsNullOrEmpty(folderPath))
                    {
                        folder = PathUtility.GetAssetPath(folderPath);
                    }
                }
                if (UnityEngine.GUILayout.Button("\u2716", UnityEngine.GUILayout.Width(20), UnityEngine.GUILayout.Height(20)))
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

                if (UnityEngine.GUILayout.Button(new GUIContent(EGUIResources.DefaultFolderIcon), UnityEngine.GUILayout.Width(20), UnityEngine.GUILayout.Height(20)))
                {
                    diskFolder = EditorUtility.OpenFolderPanel("folder", diskFolder, "");
                }
                if (UnityEngine.GUILayout.Button("\u2716", UnityEngine.GUILayout.Width(20), UnityEngine.GUILayout.Height(20)))
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

         public static bool ToolbarButton(string text,float width = 60)
        {
            return ToolbarButton(new GUIContent(text), width);
        }

        public static bool ToolbarButton(GUIContent content,float width = 60)
        {
            return UnityEngine.GUILayout.Button(content, EditorStyles.toolbarButton, UnityEngine.GUILayout.Width(width));
        }
    }
}
