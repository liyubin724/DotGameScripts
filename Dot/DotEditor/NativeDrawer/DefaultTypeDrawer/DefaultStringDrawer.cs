using System.Reflection;
using UnityEditor;

namespace DotEditor.NativeDrawer.DefaultTypeDrawer
{
    [CustomTypeDrawer(typeof(string))]
    public class DefaultStringDrawer : NativeTypeDrawer
    {
        public DefaultStringDrawer(object target, FieldInfo field) : base(target, field)
        {
        }

        protected override bool IsValid()
        {
            return ValueType == typeof(string);
        }

        protected override void OnDraw(string label)
        {
            label = label ?? "";
            string value = GetValue<string>();
            EditorGUI.BeginChangeCheck();
            {
                value = EditorGUILayout.TextField(label, value);
            }
            if (EditorGUI.EndChangeCheck())
            {
                Value = value;
            }
        }
    }
}
