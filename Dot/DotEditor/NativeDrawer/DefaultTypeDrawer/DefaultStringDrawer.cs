using System.Reflection;
using UnityEditor;

namespace DotEditor.NativeDrawer.DefaultTypeDrawer
{
    [CustomTypeDrawer(typeof(string))]
    public class DefaultStringDrawer : TypeDrawer
    {
        public DefaultStringDrawer(object target, FieldInfo field) : base(target, field)
        {
        }

        public override void OnLayoutGUI(string label)
        {
            label = label ?? "";
            string value = (string)Field.GetValue(Target);
            EditorGUI.BeginChangeCheck();
            {
                value = EditorGUILayout.TextField(label, value);
            }
            if (EditorGUI.EndChangeCheck())
            {
                Field.SetValue(Target, value);
            }
        }
    }
}
