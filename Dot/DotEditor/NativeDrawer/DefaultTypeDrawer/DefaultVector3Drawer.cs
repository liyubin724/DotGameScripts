using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace DotEditor.NativeDrawer.DefaultTypeDrawer
{
    [CustomTypeDrawer(typeof(Vector3))]
    public class DefaultVector3Drawer : NativeTypeDrawer
    {
        public DefaultVector3Drawer(object target, FieldInfo field) : base(target, field)
        {
        }

        protected override bool IsValid()
        {
            return ValueType == typeof(Vector3);
        }

        protected override void OnDraw(string label)
        {
            label = label ?? "";
            Vector3 value = GetValue<Vector3>();
            EditorGUI.BeginChangeCheck();
            {
                value = EditorGUILayout.Vector3Field(label, value);
            }
            if (EditorGUI.EndChangeCheck())
            {
                Value = value;
            }
        }
    }
}
