using System.Reflection;
using UnityEditor;

namespace DotEditor.EGUI.FieldDrawer
{
    [TargetFieldType(typeof(bool))]
    public class BoolDrawer : AFieldDrawer
    {
        public BoolDrawer(FieldInfo fieldInfo) : base(fieldInfo)
        {
        }

        protected override void OnDraw(bool isShowDesc)
        {
            bool value = (bool)fieldInfo.GetValue(data);

            EditorGUI.BeginChangeCheck();
            {
                value = EditorGUILayout.Toggle(nameContent, value);
            }
            if (EditorGUI.EndChangeCheck())
            {
                fieldInfo.SetValue(data, value);
            }
        }
    }
}
