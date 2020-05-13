using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace DotEditor.NativeDrawer.DefaultTypeDrawer
{
    [CustomTypeDrawer(typeof(Bounds))]
    public class DefaultBoundsDrawer : NativeTypeDrawer
    {
        public DefaultBoundsDrawer(object target, FieldInfo field) : base(target, field)
        {
        }

        protected override bool IsValid()
        {
            return ValueType == typeof(Bounds);
        }

        protected override void OnDraw(string label)
        {
            label = label ?? "";
            Bounds value = GetValue<Bounds>();
            EditorGUI.BeginChangeCheck();
            {
                value = EditorGUILayout.BoundsField(label, value);
            }
            if (EditorGUI.EndChangeCheck())
            {
                Value = value;
            }
        }
    }
}
