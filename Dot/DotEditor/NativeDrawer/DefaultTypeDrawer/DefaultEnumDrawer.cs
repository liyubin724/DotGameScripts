using System;
using System.Reflection;
using UnityEditor;

namespace DotEditor.NativeDrawer.DefaultTypeDrawer
{
    [CustomTypeDrawer(typeof(Enum))]
    public class DefaultEnumDrawer : NativeTypeDrawer
    {
        public DefaultEnumDrawer(object target, FieldInfo field) : base(target, field)
        {
        }

        protected override void OnDraw(string label)
        {
            label = label ?? "";
            Enum value = GetValue<Enum>();
            EditorGUI.BeginChangeCheck();
            {
                value = EditorGUILayout.EnumPopup(label, value);
            }
            if (EditorGUI.EndChangeCheck())
            {
                Value = value;
            }
        }

        protected override bool IsValid()
        {
            return ValueType == typeof(Enum);
        }
    }
}
