using Dot.Lua.Register;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DotEditor.Lua.Register
{
    [CustomPropertyDrawer(typeof(RegisterObjectData))]
    public class RegisterObjectDataPropertyDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight * 4+10;
        }

        public override bool CanCacheInspectorGUI(SerializedProperty property)
        {
            return false;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            Rect propertyRect = position;
            EditorGUI.LabelField(propertyRect, "", EditorStyles.helpBox);

            propertyRect.height = EditorGUIUtility.singleLineHeight;

            SerializedProperty obj = property.FindPropertyRelative("obj");
            SerializedProperty name = property.FindPropertyRelative("name");
            SerializedProperty regObj = property.FindPropertyRelative("regObj");
            SerializedProperty typeName = property.FindPropertyRelative("typeName");

            propertyRect.x += 4;
            propertyRect.width -= 8;
            EditorGUI.PropertyField(propertyRect, name);

            propertyRect.y += propertyRect.height;
            EditorGUI.PropertyField(propertyRect, obj);

            if (obj.objectReferenceValue == null)
            {
                regObj.objectReferenceValue = null;
                typeName.stringValue = string.Empty;
            }
            else if (string.IsNullOrEmpty(name.stringValue))
            {
                name.stringValue = obj.objectReferenceValue.name;
            }

            propertyRect.y += propertyRect.height;
            if (obj.objectReferenceValue == null)
            {
                EditorGUI.LabelField(propertyRect, "Type Name", "Null");
            }
            else
            {
                List<string> componentTypeNames = new List<string>();
                componentTypeNames.Add(typeof(GameObject).Name);

                GameObject uObj = obj.objectReferenceValue as GameObject;
                var components = uObj.GetComponents<Component>();
                foreach (var component in components)
                {
                    string componentTypeName = component.GetType().Name;
                    if (componentTypeNames.IndexOf(componentTypeName) < 0)
                    {
                        componentTypeNames.Add(componentTypeName);
                    }
                }

                int index = -1;
                if (string.IsNullOrEmpty(typeName.stringValue))
                {
                    index = 0;
                }
                else
                {
                    index = componentTypeNames.IndexOf(typeName.stringValue);
                    if (index < 0)
                    {
                        index = 0;
                    }
                }

                int newIndex = EditorGUI.Popup(propertyRect, "Type", index, componentTypeNames.ToArray());
                if (newIndex != index || string.IsNullOrEmpty(typeName.stringValue))
                {
                    typeName.stringValue = componentTypeNames[newIndex];
                    if (newIndex == 0)
                    {
                        regObj.objectReferenceValue = obj.objectReferenceValue;
                    }
                    else
                    {
                        regObj.objectReferenceValue = components[newIndex - 1];
                    }
                }
            }

            propertyRect.y += propertyRect.height;
            EditorGUI.BeginDisabledGroup(true);
            {
                EditorGUI.PropertyField(propertyRect, regObj);
            }
            EditorGUI.EndDisabledGroup();

        }
    }
}
