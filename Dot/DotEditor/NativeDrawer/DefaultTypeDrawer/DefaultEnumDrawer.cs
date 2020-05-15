using System;
using UnityEditor;

namespace DotEditor.NativeDrawer.DefaultTypeDrawer
{
    [CustomTypeDrawer(typeof(Enum))]
    public class DefaultEnumDrawer : NativeTypeDrawer
    {
        public DefaultEnumDrawer(NativeDrawerProperty property) : base(property)
        {
        }

        protected override void OnDrawProperty(string label)
        {
            label = label ?? "";
            Enum value = DrawerProperty.GetValue<Enum>();
            EditorGUI.BeginChangeCheck();
            {
                value = EditorGUILayout.EnumPopup(label, value);
            }
            if (EditorGUI.EndChangeCheck())
            {
                DrawerProperty.Value = value;
            }
        }

        protected override bool IsValidProperty()
        {
            return typeof(Enum).IsAssignableFrom(DrawerProperty.ValueType);
        }
    }
}
