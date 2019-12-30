using System.Reflection;
using UnityEditor;

namespace DotEditor.EGUI.FieldDrawer
{
    [TargetFieldType(typeof(int))]
    public class IntDrawer : AFieldDrawer
    {
        public IntDrawer(FieldInfo fieldInfo) : base(fieldInfo)
        {
        }

        protected override void OnDraw(object data, bool isShowDesc)
        {
            int value = (int)fieldInfo.GetValue(data);

            EditorGUI.BeginChangeCheck();
            {
                value = EditorGUILayout.IntField(fieldInfo.Name, value);
            }
            if(EditorGUI.EndChangeCheck())
            {
                fieldInfo.SetValue(data, value);
            }
        }
    }
}
