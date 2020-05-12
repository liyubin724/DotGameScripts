using System.Reflection;
using UnityEditor;

namespace DotEditor.NativeDrawer.DefaultTypeDrawer
{
    [CustomTypeDrawer(typeof(int))]
    public class DefaultIntDrawer : TypeDrawer
    {
        public DefaultIntDrawer(object target, FieldInfo field) : base(target, field)
        {
        }

        public override void OnLayoutGUI(string label)
        {
            label = label ?? "";
            int value = (int)Field.GetValue(Target);
            EditorGUI.BeginChangeCheck();
            {
                value = EditorGUILayout.IntField(label, value);
            }
            if(EditorGUI.EndChangeCheck())
            {
                Field.SetValue(Target, value);
            }
        }
    }
}
