using System.Reflection;
using UnityEditor;

namespace DotEditor.NativeDrawer.DefaultTypeDrawer
{
    [CustomTypeDrawer(typeof(float))]
    public class DefaultFloatDrawer : TypeDrawer
    {
        public DefaultFloatDrawer(object target, FieldInfo field) : base(target, field)
        {
        }

        public override void OnLayoutGUI(string label)
        {
            label = label ?? "";
            float value = (float)Field.GetValue(Target);
            EditorGUI.BeginChangeCheck();
            {
                value = EditorGUILayout.FloatField(label, value);
            }
            if (EditorGUI.EndChangeCheck())
            {
                Field.SetValue(Target, value);
            }
        }
    }
}
