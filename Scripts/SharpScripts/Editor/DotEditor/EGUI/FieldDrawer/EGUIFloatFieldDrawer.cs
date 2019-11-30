using DotEditor.EGUI.FieldDrawer.Attributes;
using System.Reflection;
using UnityEditor;

namespace DotEditor.EGUI.FieldDrawer
{
    [FieldDrawerType(typeof(float))]
    public class EGUIFloatFieldDrawer : AEGUIFieldDrawer
    {
        public EGUIFloatFieldDrawer(object data, FieldInfo fieldInfo, bool isShowDesc) : base(data, fieldInfo, isShowDesc)
        {
        }

        protected override object DrawField()
        {
            dynamic value = m_FieldInfo.GetValue(m_Data);
            return EditorGUILayout.FloatField(m_FieldInfo.Name, value);
        }
    }
}
