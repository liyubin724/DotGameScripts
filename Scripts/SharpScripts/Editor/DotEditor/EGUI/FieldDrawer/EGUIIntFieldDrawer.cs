using DotEditor.EGUI.FieldDrawer.Attributes;
using System.Reflection;
using UnityEditor;
using SystemObject = System.Object;

namespace DotEditor.EGUI.FieldDrawer
{
    [FieldDrawerType(typeof(int))]
    public class EGUIIntFieldDrawer : AEGUIFieldDrawer
    {
        public EGUIIntFieldDrawer(object data, FieldInfo fieldInfo, bool isShowDesc) : base(data, fieldInfo, isShowDesc)
        {
        }

        protected override SystemObject DrawField()
        {
            dynamic value = m_FieldInfo.GetValue(m_Data);
            return EditorGUILayout.IntField(m_FieldInfo.Name, value);
        }
    }
}
