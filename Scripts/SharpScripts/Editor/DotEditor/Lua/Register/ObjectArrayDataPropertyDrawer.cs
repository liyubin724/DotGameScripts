using Dot.Lua.Register;
using Rotorz.Games.Collections;
using UnityEditor;
using UnityEngine;

namespace DotEditor.Lua.Register
{
    //[CustomPropertyDrawer(typeof(ObjectArrayData))]
    public class ObjectArrayDataPropertyDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            SerializedProperty name = property.FindPropertyRelative("name");
            SerializedProperty objects = property.FindPropertyRelative("objects");

            float height = EditorGUIUtility.singleLineHeight;
            if(objects.arraySize == 0)
            {
                height += 60;
            }else
            {
                height += 40+objects.arraySize * EditorGUI.GetPropertyHeight(objects.GetArrayElementAtIndex(0), GUIContent.none, false);
            }
            return height;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            Rect propertyRect = position;
            propertyRect.height = EditorGUIUtility.singleLineHeight;

            SerializedProperty name = property.FindPropertyRelative("name");
            EditorGUI.PropertyField(propertyRect, name);

            propertyRect.y += EditorGUIUtility.singleLineHeight;
            ReorderableListGUI.Title(propertyRect, "objects");
                
            SerializedProperty objects = property.FindPropertyRelative("objects");

            propertyRect = new Rect(
                position.x,
                propertyRect.y+propertyRect.height,
                position.width,
                position.height - propertyRect.y - propertyRect.height);

            ReorderableListGUI.ListFieldAbsolute(propertyRect, objects);
        }
    }
}