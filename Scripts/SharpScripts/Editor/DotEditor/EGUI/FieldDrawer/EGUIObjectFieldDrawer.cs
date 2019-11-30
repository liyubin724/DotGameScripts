using DotEditor.Core.EGUI;
using DotEditor.EGUI.FieldDrawer.Attributes;
using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using SystemObject = System.Object;

namespace DotEditor.EGUI.FieldDrawer
{
    [FieldDrawerType(typeof(SystemObject))]
    public class EGUIObjectFieldDrawer : AEGUIFieldDrawer
    {
        private bool m_IsFoldout = false;
        private EGUIObjectDrawer objectDrawer = null;

        public EGUIObjectFieldDrawer(object data, FieldInfo fieldInfo, bool isShowDesc) : base(data, fieldInfo, isShowDesc)
        {
        }

        protected override object DrawField()
        {
            var value = m_FieldInfo.GetValue(m_Data);

            m_IsFoldout = EditorGUILayout.Foldout(m_IsFoldout, FieldName, true);
            if(m_IsFoldout)
            {
                object newValue = null;
                EditorGUIUtil.BeginIndent();
                {
                    if (value == null)
                    {
                        EditorGUILayout.BeginHorizontal();
                        {
                            EditorGUILayout.LabelField("NULL");
                            if(GUILayout.Button("New",GUILayout.Width(60)))
                            {
                                newValue = Activator.CreateInstance(FieldType);
                            }
                        }
                        EditorGUILayout.EndHorizontal();
                    }
                    else
                    {
                        newValue = value;
                        if (objectDrawer == null)
                        {
                            objectDrawer = new EGUIObjectDrawer(value, m_IsShowDesc);
                        }
                        objectDrawer.OnGUILayout();
                    }
                }
                EditorGUIUtil.EndIndent();
                return newValue;
            }
            return value;
        }
    }
}
