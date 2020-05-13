using System.Reflection;
using UnityEditor;

namespace DotEditor.NativeDrawer.DefaultTypeDrawer
{
    [CustomTypeDrawer(typeof(float))]
    public class DefaultFloatDrawer : NativeTypeDrawer
    {
        public DefaultFloatDrawer(object target, FieldInfo field) : base(target, field)
        {
        }

        protected override bool IsValid()
        {
            return ValueType == typeof(float);
        }

        protected override void OnDraw(string label)
        {
            label = label ?? "";
            float value = GetValue<float>();
            EditorGUI.BeginChangeCheck();
            {
                value = EditorGUILayout.FloatField(label, value);
            }
            if (EditorGUI.EndChangeCheck())
            {
                Value = value;
            }
        }
    }
}
