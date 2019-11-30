using Dot.FieldDrawer.Attributes;
using System;
using System.Reflection;
using UnityEditor;
using SystemObject = System.Object;

namespace DotEditor.EGUI.FieldDrawer
{
    public abstract class AEGUIFieldDrawer
    {
        protected object m_Data;
        protected FieldInfo m_FieldInfo;
        protected bool m_IsShowDesc;

        protected Type FieldType { get => m_FieldInfo.FieldType; }
        protected string FieldName { get => m_FieldInfo.Name; }
        public FieldInfo FieldInfo { get => m_FieldInfo; }
        public virtual bool IsShowDesc { set =>m_IsShowDesc = value;  }

        public AEGUIFieldDrawer(object data, FieldInfo fieldInfo, bool isShowDesc)
        {
            m_Data = data;
            m_FieldInfo = fieldInfo;
            m_IsShowDesc = isShowDesc;
        }

        protected virtual void DrawDesc()
        {
            MemberDesc memberDesc = m_FieldInfo.GetCustomAttribute<MemberDesc>();
            if (memberDesc != null && m_IsShowDesc)
            {
                EditorGUILayout.HelpBox(memberDesc.Desc,MessageType.Info);
            }
        }

        public void OnGUILayout()
        {
            DrawDesc();

            FieldReadonly fieldReadonlyAttr = m_FieldInfo.GetCustomAttribute<FieldReadonly>();
            EditorGUI.BeginDisabledGroup(fieldReadonlyAttr != null);
            {
                SystemObject newValue = DrawField();

                if(fieldReadonlyAttr == null)
                {
                    m_FieldInfo.SetValue(m_Data, newValue);
                }
            }
            EditorGUI.EndDisabledGroup();
        }

        protected abstract SystemObject DrawField();

    }
}
