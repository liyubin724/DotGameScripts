using DotEditor.Core.EGUI;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using System;

namespace DotEditor.EGUI.FieldDrawer
{
    [TargetFieldType(typeof(object))]
    public class DefalutClassDrawer : AFieldDrawer
    {
        private FieldData[] fieldDatas = null;
        private bool isFoldout = false;
        public DefalutClassDrawer(FieldInfo fieldInfo) : base(fieldInfo)
        {
            fieldDatas = FieldDrawerUtil.GetTypeFieldDrawer(fieldInfo.FieldType);
        }

        protected override void OnDraw(object data, bool isShowDesc)
        {
            isFoldout = EditorGUILayout.Foldout(isFoldout, fieldInfo.Name, true);
            if(isFoldout)
            {
                object innerData = fieldInfo.GetValue(data);
                
                EditorGUIUtil.BeginIndent();
                {
                    if (innerData == null)
                    {
                        EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);
                        {
                            EditorGUILayout.LabelField("Data is null");
                            if (GUILayout.Button("New", GUILayout.Width(40)))
                            {
                                innerData = Activator.CreateInstance(fieldInfo.FieldType);
                                fieldInfo.SetValue(data, innerData);
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
                                    fieldData.drawer.DrawField(innerData, isShowDesc);
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
