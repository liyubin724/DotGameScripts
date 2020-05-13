using System.Reflection;
using UnityEditor;

namespace DotEditor.NativeDrawer.DefaultTypeDrawer
{
    [CustomTypeDrawer(typeof(bool))]
    public class DefaultBoolDrawer : NativeTypeDrawer
    {
        public DefaultBoolDrawer(object target, FieldInfo field) : base(target, field)
        {
        }

        protected override void OnDraw(string label)
        {
            label = label ?? "";
            bool value = GetValue<bool>();
            EditorGUI.BeginChangeCheck();
            {
                value = EditorGUILayout.Toggle(label, value);
            }
            if (EditorGUI.EndChangeCheck())
            {
                Value = value;
            }
        }

        protected override bool IsValid()
        {
            return ValueType == typeof(bool);
        }
    }
}
