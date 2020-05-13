using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace DotEditor.NativeDrawer.DefaultTypeDrawer
{
    [CustomTypeDrawer(typeof(Rect))]
    public class DefaultRectDrawer : NativeTypeDrawer
    {
        public DefaultRectDrawer(object target, FieldInfo field) : base(target, field)
        {
        }

        protected override bool IsValid()
        {
            return ValueType == typeof(Rect);
        }

        protected override void OnDraw(string label)
        {
            label = label ?? "";
            Rect value = GetValue<Rect>();
            EditorGUI.BeginChangeCheck();
            {
                value = EditorGUILayout.RectField(label, value);
            }
            if (EditorGUI.EndChangeCheck())
            {
                Value = value;
            }
        }
    }
}
