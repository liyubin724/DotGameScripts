using Dot.NativeDrawer.Property;
using DotEditor.GUIExtension;
using DotEditor.Utilities;
using UnityEditor;
using UnityEngine;

namespace DotEditor.NativeDrawer.Property
{
    [CustomAttributeDrawer(typeof(OpenFilePathAttribute))]
    public class OpenFilePathDrawer : PropertyDrawer
    {
        public OpenFilePathDrawer(NativeDrawerProperty drawerProperty, PropertyDrawerAttribute attr) : base(drawerProperty, attr)
        {
        }

        protected override bool IsValidProperty()
        {
            return DrawerProperty.ValueType == typeof(string);
        }

        protected override void OnDrawProperty(string label)
        {
            string value = DrawerProperty.GetValue<string>();
            var attr = GetAttr<OpenFilePathAttribute>();

            EditorGUI.BeginChangeCheck();
            {
                EditorGUILayout.BeginHorizontal();
                {
                    value = EditorGUILayout.TextField(label, value);

                    if (GUILayout.Button(new GUIContent(EGUIResources.DefaultFolderIcon),GUIStyle.none ,GUILayout.Width(17),GUILayout.Height(17)))
                    {
                        string filePath;
                        if (attr.Filters!=null && attr.Filters.Length>0)
                        {
                            filePath = EditorUtility.OpenFilePanelWithFilters("Select file", "", attr.Filters);
                        }
                        else
                        {
                            filePath = EditorUtility.OpenFilePanel("Select file", "", attr.Extension);
                        }
                        if (!string.IsNullOrEmpty(filePath))
                        {
                            if (attr.IsAbsolute)
                            {
                                value = filePath.Replace("\\", "/");
                            }
                            else
                            {
                                value = PathUtility.GetAssetPath(filePath);
                            }
                        }
                    }
                }
                EditorGUILayout.EndHorizontal();
            }
            if(EditorGUI.EndChangeCheck())
            {
                DrawerProperty.Value = value;
            }
        }
    }
}
