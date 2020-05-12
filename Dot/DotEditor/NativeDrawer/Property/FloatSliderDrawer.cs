using Dot.NativeDrawer;
using Dot.NativeDrawer.Property;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace DotEditor.NativeDrawer.Property
{
    [CustomAttDrawerLink(typeof(FloatSliderAttribute))]
    public class FloatSliderDrawer : PropertyDrawer
    {
        public FloatSliderDrawer(object target, FieldInfo field, NativeDrawerAttribute attr) : base(target, field, attr)
        {
        }

        protected override void OnProperty(string label)
        {
            FloatSliderAttribute attr = GetAttr<FloatSliderAttribute>();

            label = label ?? "";

            float value = (float)Field.GetValue(Target);
            EditorGUI.BeginChangeCheck();
            {
                EditorGUILayout.BeginHorizontal();
                {
                    value = EditorGUILayout.Slider(label,value, attr.LeftValue, attr.RightValue);
                    if(attr.ShowTextField)
                    {
                        value = EditorGUILayout.FloatField(value, GUILayout.Width(40));
                    }
                }
                EditorGUILayout.EndHorizontal();
            }
            if(EditorGUI.EndChangeCheck())
            {
                Field.SetValue(Target, value);
            }
        }

        protected override bool IsValid()
        {
            return Field.FieldType == typeof(float);
        }
    }
}
