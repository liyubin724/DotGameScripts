using Dot.Lua.Register;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace DotEditor.Lua.Register
{
    [CustomPropertyDrawer(typeof(RegisterObjectArrayData))]
    public class RegisterObjectArrayDataPropertyDrawer : PropertyDrawer
    {
        private Dictionary<string, ReorderableList> listDic = new Dictionary<string, ReorderableList>();
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            SerializedProperty objects = property.FindPropertyRelative("objects");

            float height = EditorGUIUtility.singleLineHeight;
            if(objects.arraySize == 0)
            {
                height += 70;
            }else
            {
                height += objects.arraySize * EditorGUIUtility.singleLineHeight * 4 + 50;
            }

            return height;
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

            SerializedProperty name = property.FindPropertyRelative("name");
            SerializedProperty objects = property.FindPropertyRelative("objects");

            propertyRect.x += 4;
            propertyRect.width -= 8;
            EditorGUI.PropertyField(propertyRect, name);

            propertyRect.y += EditorGUIUtility.singleLineHeight + 4;
            propertyRect.height = position.height - EditorGUIUtility.singleLineHeight;
            propertyRect.x += 14;
            propertyRect.width -= 14;

            string pPath = objects.propertyPath;
            if (!listDic.TryGetValue(pPath, out ReorderableList objectList))
            {
                objectList = new ReorderableList(objects.serializedObject, objects, true, true, true, true);
                objectList.drawHeaderCallback = (rect) =>
                {
                    EditorGUI.LabelField(rect, "Objects");
                };
                objectList.drawElementCallback = (rect, index, isActive, isFocused) =>
                {
                    EditorGUI.PropertyField(rect, objects.GetArrayElementAtIndex(index));
                };
                objectList.elementHeightCallback = (index) =>
                {
                    return EditorGUIUtility.singleLineHeight * 4;
                };

                listDic.Add(pPath, objectList);
            }

            objectList.DoList(propertyRect);
        }
    }
}
