using DotEditor.Core.EGUI;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using System;
using SystemObject = System.Object;

namespace DotEditor.EGUI.FieldDrawer
{
    [TargetFieldType(typeof(object))]
    public class DefalutClassDrawer : AFieldDrawer
    {
        private FieldData[] fieldDatas = null;
        private bool isFoldout = false;
        private SystemObject valueObject = null;
        public DefalutClassDrawer(FieldInfo fieldInfo) : base(fieldInfo)
        {
            fieldDatas = FieldDrawerUtil.GetTypeFieldDrawer(fieldInfo.FieldType);
        }

        public override void SetData(object data)
        {
            base.SetData(data);
            valueObject = fieldInfo.GetValue(data);

            if(valueObject!=null)
            {
                foreach(var fd in fieldDatas)
                {
                    if(fd.drawer!=null)
                    {
                        fd.drawer.SetData(valueObject);
                    }
                }
            }

        }

        protected override void OnDraw(bool isShowDesc)
        {
            isFoldout = EditorGUILayout.Foldout(isFoldout, nameContent, true);
            if(isFoldout)
            {
                EditorGUIUtil.BeginIndent();
                {
                    if (valueObject == null)
                    {
                        EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);
                        {
                            EditorGUILayout.LabelField("Data is null");
                            if (GUILayout.Button("New", GUILayout.Width(40)))
                            {
                                valueObject = Activator.CreateInstance(fieldInfo.FieldType);
                                fieldInfo.SetValue(data, valueObject);

                                SetData(data);
                            }
                        }
                        EditorGUILayout.EndHorizontal();
                    }
                    else
                    {
                        EditorGUILayout.BeginVertical();
                        {
                            foreach (var fieldData in fieldDatas)
                            {
                                if (fieldData.drawer == null)
                                {
                                    EditorGUILayout.LabelField(fieldData.name, "Drawer is null");
                                }
                                else
                                {
                                    fieldData.drawer.DrawField(isShowDesc);
                                }
                            }
                        }
                        EditorGUILayout.EndVertical();
                    }
                    
                }
                EditorGUIUtil.EndIndent();

            }
        }
    }
}
