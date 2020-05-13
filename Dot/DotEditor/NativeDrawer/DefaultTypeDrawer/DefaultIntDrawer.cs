using System.Reflection;
using UnityEditor;

namespace DotEditor.NativeDrawer.DefaultTypeDrawer
{
    [CustomTypeDrawer(typeof(int))]
    public class DefaultIntDrawer : NativeTypeDrawer
    {
        public DefaultIntDrawer(object target, FieldInfo field) : base(target, field)
        {
        }

        protected override bool IsValid()
        {
            return ValueType == typeof(int);
        }

        protected override void OnDraw(string label)
        {
            label = label ?? "";
            int value = GetValue<int>();
            EditorGUI.BeginChangeCheck();
            {
                value = EditorGUILayout.IntField(label, value);
            }
            if(EditorGUI.EndChangeCheck())
            {
                Value = value;
            }
        }
    }
}
