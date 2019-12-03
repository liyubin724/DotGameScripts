using Dot.Lua.Register;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace DotEditor.Lua.Register
{
    [CustomPropertyDrawer(typeof(RegisterBehaviourArrayData),false)]
    public class RegisterBehaviourArrayDataPropertyDrawer : PropertyDrawer
    {
        private Dictionary<string, ReorderableList> listDic = new Dictionary<string, ReorderableList>();
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            SerializedProperty behaviours = property.FindPropertyRelative("behaviours");

            float height = EditorGUIUtility.singleLineHeight;
            if (behaviours.arraySize == 0)
            {
                height += 70;
            }
            else
            {
                height += behaviours.arraySize * EditorGUIUtility.singleLineHeight + 50;
            }

            return height;
        }

        public override bool CanCacheInspectorGUI(SerializedProperty property)
        {
            return false;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.LabelField(position, "", EditorStyles.helpBox);

            Rect propertyRect = position;
            propertyRect.height = EditorGUIUtility.singleLineHeight;

            SerializedProperty name = property.FindPropertyRelative("name");
            SerializedProperty behaviours = property.FindPropertyRelative("behaviours");

            propertyRect.x += 4;
            propertyRect.width -= 8;
            EditorGUI.PropertyField(propertyRect, name);

            propertyRect.y += EditorGUIUtility.singleLineHeight + 4;
            propertyRect.height = position.height - EditorGUIUtility.singleLineHeight;
            propertyRect.x += 14;
            propertyRect.width -= 14;

            string pPath = behaviours.propertyPath;
            if (!listDic.TryGetValue(pPath, out ReorderableList behaviourList))
            {
                behaviourList = new ReorderableList(behaviours.serializedObject, behaviours, true, true, true, true);
                behaviourList.drawHeaderCallback = (rect) =>
                {
                    EditorGUI.LabelField(rect, "Behaviours");
                };
                behaviourList.drawElementCallback = (rect, index, isActive, isFocused) =>
                {
                    EditorGUI.PropertyField(rect, behaviours.GetArrayElementAtIndex(index));
                };
                behaviourList.onAddCallback = (list) =>
                {
                    int insertIndex = list.serializedProperty.arraySize;
                    list.serializedProperty.InsertArrayElementAtIndex(insertIndex);
                };

                listDic.Add(pPath, behaviourList);
            }
            behaviourList.DoList(propertyRect);
        }
    }
}
