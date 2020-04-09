using Dot.GUI.Attributes;
using UnityEditor;
using UnityEngine;

namespace DotEditor.EGUI.Drawers
{
    public abstract class EGUIPropertyDrawer : PropertyDrawer
    {
        protected virtual void OnGUISafe(Rect position, SerializedProperty property,GUIContent label)
        {
            EditorGUI.PropertyField(position, property, label);
        }

        public override sealed void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if(IsPropertyValid(property))
            {
                EditorGUI.PropertyField(position, property, label);
            }else
            {

            }
        }

        public virtual bool IsPropertyValid(SerializedProperty property)
        {
            return true;
        }

        protected T GetAttribute<T>() where T : EGUIPropertyAttribute
        {
            return (T)attribute;
        }
    }
}
