using System;
using System.Reflection;
using UnityEditor;

namespace DotEditor.EGUI.FieldDrawer
{
    [TargetFieldType(typeof(Enum))]
    public class EnumDrawer : AFieldDrawer
    {
        public EnumDrawer(FieldInfo fieldInfo) : base(fieldInfo)
        {
        }

        protected override void OnDraw(bool isShowDesc)
        {
            object value = (Enum)fieldInfo.GetValue(data);

            EditorGUI.BeginChangeCheck();
            {
                value = EditorGUILayout.EnumPopup(fieldInfo.Name, (Enum)value);
            }
            if (EditorGUI.EndChangeCheck())
            {
                fieldInfo.SetValue(data, value);
            }
        }
    }
}
