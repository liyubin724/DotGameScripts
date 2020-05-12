using Dot.NativeDrawer;
using Dot.NativeDrawer.Property;
using System.Reflection;
using UnityEditor;

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
                value = EditorGUILayout.Slider(label,value, attr.LeftValue, attr.RightValue);
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
