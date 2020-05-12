using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace DotEditor.NativeDrawer.DefaultTypeDrawer
{
    [CustomTypeDrawer(typeof(Rect))]
    public class DefaultRectDrawer : TypeDrawer
    {
        public DefaultRectDrawer(object target, FieldInfo field) : base(target, field)
        {
        }

        public override void OnLayoutGUI(string label)
        {
            label = label ?? "";
            Rect value = (Rect)Field.GetValue(Target);
            EditorGUI.BeginChangeCheck();
            {
                value = EditorGUILayout.RectField(label, value);
            }
            if (EditorGUI.EndChangeCheck())
            {
                Field.SetValue(Target, value);
            }
        }
    }
}
