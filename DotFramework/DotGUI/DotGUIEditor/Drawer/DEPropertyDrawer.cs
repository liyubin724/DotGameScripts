using UnityEditor;
using UnityEngine;

namespace DotEditor.EGUI.Drawer
{
    public abstract class DEPropertyDrawer : PropertyDrawer
    {
        protected virtual void DrawProperty(Rect position,SerializedProperty property,GUIContent label)
        {
            EditorGUI.PropertyField(position, property, label);
        }

        protected virtual void DrawInvalidProperty(Rect position,SerializedProperty property)
        {

        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if(IsPropertyValid(property))
            {
                DrawProperty(position, property, label);
            }else
            {
                DrawInvalidProperty(position, property);
            }
        }

        public virtual bool IsPropertyValid(SerializedProperty property)
        {
            return true;
        }
    }
}
