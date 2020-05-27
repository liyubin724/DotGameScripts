using Dot.NativeDrawer.Property;
using DotEditor.GUIExtension;
using DotEditor.Utilities;
using UnityEditor;
using UnityEngine;

namespace DotEditor.NativeDrawer.Property
{
    [CustomAttributeDrawer(typeof(OpenFolderPathAttribute))]
    public class OpenFolderPathDrawer : PropertyDrawer
    {
        public OpenFolderPathDrawer(NativeDrawerProperty drawerProperty, PropertyDrawerAttribute attr) : base(drawerProperty, attr)
        {
        }

        protected override bool IsValidProperty()
        {
            return DrawerProperty.ValueType == typeof(string);
        }

        protected override void OnDrawProperty(string label)
        {
            string value = DrawerProperty.GetValue<string>();
            var attr = GetAttr<OpenFolderPathAttribute>();

            EditorGUI.BeginChangeCheck();
            {
                EditorGUILayout.BeginHorizontal();
                {
                    value = EditorGUILayout.TextField(label, value);

                    if (GUILayout.Button(new GUIContent(EGUIResources.DefaultFolderIcon), GUIStyle.none, GUILayout.Width(17), GUILayout.Height(17)))
                    {
                        string folderPath = EditorUtility.OpenFolderPanel("Open Folder", value,"");
                        if (!string.IsNullOrEmpty(folderPath))
                        {
                            if (attr.IsAbsolute)
                            {
                                value = folderPath.Replace("\\", "/");
                            }
                            else
                            {
                                value = PathUtility.GetAssetPath(folderPath);
                            }
                        }
                    }
                }
                EditorGUILayout.EndHorizontal();
            }
            if (EditorGUI.EndChangeCheck())
            {
                DrawerProperty.Value = value;
            }
        }
    }
}
