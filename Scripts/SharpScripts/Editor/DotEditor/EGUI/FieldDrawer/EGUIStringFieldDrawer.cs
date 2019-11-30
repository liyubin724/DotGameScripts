using Dot.FieldDrawer.Attributes;
using DotEditor.EGUI.FieldDrawer.Attributes;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace DotEditor.EGUI.FieldDrawer
{
    [FieldDrawerType(typeof(string))]
    public class EGUIStringFieldDrawer : AEGUIFieldDrawer
    {
        private GUIStyle warpTextStyle = null;
        public EGUIStringFieldDrawer(object data, FieldInfo fieldInfo, bool isShowDesc) : base(data, fieldInfo, isShowDesc)
        {
            warpTextStyle = new GUIStyle(EditorStyles.textArea);
            warpTextStyle.wordWrap = true;
        }

        protected override object DrawField()
        {
            dynamic value = m_FieldInfo.GetValue(m_Data);
            if(value == null)
            {
                value = string.Empty;
            }

            FieldMultilineText multilineTextAttr = m_FieldInfo.GetCustomAttribute<FieldMultilineText>();
            if(multilineTextAttr!=null)
            {
                EditorGUILayout.LabelField(FieldName);
                return EditorGUILayout.TextArea(value, warpTextStyle, GUILayout.Height(55));
            }
            else
            {
                return EditorGUILayout.TextField(m_FieldInfo.Name, value);
            }
        }
    }
}
