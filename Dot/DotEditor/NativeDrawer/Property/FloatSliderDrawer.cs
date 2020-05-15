using Dot.NativeDrawer.Property;
using UnityEditor;

namespace DotEditor.NativeDrawer.Property
{
    [CustomAttributeDrawer(typeof(FloatSliderAttribute))]
    public class FloatSliderDrawer : PropertyDrawer
    {
        public FloatSliderDrawer(NativeDrawerProperty drawerProperty, PropertyDrawerAttribute attr) : base(drawerProperty, attr)
        {
        }

        protected override void OnDrawProperty(string label)
        {
            FloatSliderAttribute attr = GetAttr<FloatSliderAttribute>();

            label = label ?? "";

            float value = DrawerProperty.GetValue<float>();
            EditorGUI.BeginChangeCheck();
            {
                value = EditorGUILayout.Slider(label, value, attr.LeftValue, attr.RightValue);
            }
            if(EditorGUI.EndChangeCheck())
            {
                DrawerProperty.Value = value;
            }
        }

        protected override bool IsValidProperty()
        {
            return DrawerProperty.ValueType == typeof(float);
        }
    }
}
