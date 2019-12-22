using Dot.Lua.Event;
using Rotorz.Games.Collections;
using UnityEditor;
using UnityEngine;

namespace DotEditor.Lua.Event
{
    [CustomPropertyDrawer(typeof(LuaEventData))]
    public class LuaEventDataPropertyDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            SerializedProperty eventParams = property.FindPropertyRelative("eventParams");
            float height = ReorderableListGUI.DefaultItemHeight * 4;
            if(eventParams.arraySize == 0)
            {
                height += ReorderableListGUI.DefaultItemHeight * 4;
            }else
            {
                height += eventParams.arraySize* (4 * ReorderableListGUI.DefaultItemHeight +6) +40;
            }
            return height;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            Rect curRect = position;
            curRect.height = ReorderableListGUI.DefaultItemHeight;

            EditorGUI.LabelField(curRect, label, EditorStyles.boldLabel);

            curRect.y += curRect.height;
            curRect.x += 12;

            SerializedProperty bindBehaviour = property.FindPropertyRelative("bindBehaviour");
            EditorGUI.PropertyField(curRect, bindBehaviour);

            curRect.y += curRect.height;
            SerializedProperty funcName = property.FindPropertyRelative("funcName");
            EditorGUI.PropertyField(curRect, funcName);

            curRect.y += curRect.height;
            ReorderableListGUI.Title(curRect, "Event Params");

            curRect.y += curRect.height;
            curRect.height = position.height + position.y - curRect.y;

            SerializedProperty eventParams = property.FindPropertyRelative("eventParams");
            ReorderableListGUI.ListFieldAbsolute(curRect,eventParams);
        }
    }
}
