using DotEditor.EGUI.FieldDrawer.Attributes;
using System.Reflection;
using UnityEditor;

namespace DotEditor.EGUI.FieldDrawer
{
    [FieldDrawerType(typeof(bool))]
    public class EGUIBoolFieldDrawer : AEGUIFieldDrawer
    {
        public EGUIBoolFieldDrawer(object data, FieldInfo fieldInfo, bool isShowDesc) : base(data, fieldInfo, isShowDesc)
        {
        }

        protected override object DrawField()
        {
            dynamic value = m_FieldInfo.GetValue(m_Data);
            return EditorGUILayout.Toggle(m_FieldInfo.Name, value);
        }
    }
}
