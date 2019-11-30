using DotEditor.EGUI.FieldDrawer.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;

namespace DotEditor.EGUI.FieldDrawer
{
    [FieldDrawerType(typeof(UnityEngine.Object))]
    public class EGUIUnityFieldDrawer : AEGUIFieldDrawer
    {
        public EGUIUnityFieldDrawer(object data, FieldInfo fieldInfo, bool isShowDesc) : base(data, fieldInfo, isShowDesc)
        {
        }

        protected override object DrawField()
        {
            dynamic value = m_FieldInfo.GetValue(m_Data);

            return EditorGUILayout.ObjectField(FieldName, value, FieldType, true);
        }
    }
}
