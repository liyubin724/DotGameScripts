using Dot.NativeDrawer.Property;
using DotEditor.GUIExtension;
using DotEditor.Utilities;
using UnityEditor;
using UnityEngine;

namespace DotEditor.NativeDrawer.Property
{
    [CustomAttributeDrawer(typeof(FilePathAttribute))]
    public class FilePathDrawer : PropertyDrawer
    {
        public FilePathDrawer(NativeDrawerProperty drawerProperty, PropertyDrawerAttribute attr) : base(drawerProperty, attr)
        {
        }

        protected override bool IsValidProperty()
        {
            return DrawerProperty.ValueType == typeof(string);
        }

        protected override void OnDrawProperty(string label)
        {
            string value = DrawerProperty.GetValue<string>();
            var attr = GetAttr<FilePathAttribute>();

            EditorGUI.BeginChangeCheck();
            {
                EditorGUILayout.BeginHorizontal();
                {
                    value = EditorGUILayout.TextField(label, value);

                    if (GUILayout.Button(new GUIContent(EGUIResources.DefaultFolderIcon), GUILayout.Width(20)))
                    {
                        string filePath = EditorUtility.OpenFilePanel("Select file", value, null);
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
                    if (UnityEngine.GUILayout.Button("\u2716", UnityEngine.GUILayout.Width(20), UnityEngine.GUILayout.Height(20)))
                    {
                        value = "";
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
