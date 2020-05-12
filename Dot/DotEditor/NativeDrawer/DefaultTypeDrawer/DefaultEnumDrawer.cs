using System;
using System.Reflection;
using UnityEditor;

namespace DotEditor.NativeDrawer.DefaultTypeDrawer
{
    [CustomTypeDrawer(typeof(Enum))]
    public class DefaultEnumDrawer : TypeDrawer
    {
        public DefaultEnumDrawer(object target, FieldInfo field) : base(target, field)
        {
        }

        public override void OnLayoutGUI(string label)
        {
            label = label ?? "";
            Enum value = (Enum)Field.GetValue(Target);
            EditorGUI.BeginChangeCheck();
            {
                value = EditorGUILayout.EnumPopup(label, value);
            }
            if (EditorGUI.EndChangeCheck())
            {
                Field.SetValue(Target, value);
            }
        }
    }
}
