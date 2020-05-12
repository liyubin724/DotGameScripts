using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace DotEditor.NativeDrawer.DefaultTypeDrawer
{
    [CustomTypeDrawer(typeof(Vector3))]
    public class DefaultVector3Drawer : TypeDrawer
    {
        public DefaultVector3Drawer(object target, FieldInfo field) : base(target, field)
        {
        }

        public override void OnLayoutGUI(string label)
        {
            label = label ?? "";
            Vector3 value = (Vector3)Field.GetValue(Target);
            EditorGUI.BeginChangeCheck();
            {
                value = EditorGUILayout.Vector3Field(label, value);
            }
            if (EditorGUI.EndChangeCheck())
            {
                Field.SetValue(Target, value);
            }
        }
    }
}
