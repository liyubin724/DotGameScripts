using DotEditor.EGUI.FieldDrawer.Attributes;
using System;
using System.Reflection;
using UnityEditor;

namespace DotEditor.EGUI.FieldDrawer
{
    [FieldDrawerType(typeof(Enum))]
    public class EGUIEnumFieldDrawer : AEGUIFieldDrawer
    {
        public EGUIEnumFieldDrawer(object data, FieldInfo fieldInfo, bool isShowDesc) : base(data, fieldInfo, isShowDesc)
        {
        }

        protected override object DrawField()
        {
            dynamic value = m_FieldInfo.GetValue(m_Data);

            FlagsAttribute flagsAttr = FieldType.GetCustomAttribute<FlagsAttribute>();
            if(flagsAttr == null)
            {
                return EditorGUILayout.EnumPopup(FieldName, value);
            }else
            {
                return EditorGUILayout.EnumMaskField(FieldName, value);
            }

        }
    }
}
