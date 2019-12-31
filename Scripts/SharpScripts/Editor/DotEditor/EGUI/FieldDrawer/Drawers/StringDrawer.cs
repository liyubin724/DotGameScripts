using Dot.FieldDrawer;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace DotEditor.EGUI.FieldDrawer
{
    [TargetFieldType(typeof(string))]
    public class StringDrawer : AFieldDrawer
    {
        private bool isMultilineText = false;
        private int multilineHeight = 0;
        public StringDrawer(FieldInfo fieldInfo) : base(fieldInfo)
        {
            FieldMultilineText attr = fieldInfo.GetCustomAttribute<FieldMultilineText>();
            if(attr!=null)
            {
                isMultilineText = true;
                multilineHeight = attr.Height;
            }
        }

        protected override void OnDraw(bool isReadonly, bool isShowDesc)
        {
            string value = (string)fieldInfo.GetValue(data);

            EditorGUILayout.BeginHorizontal();
            {
                EditorGUI.BeginDisabledGroup(isReadonly);
                {
                    EditorGUI.BeginChangeCheck();
                    {
                        if (isMultilineText)
                        {
                            EditorGUILayout.BeginHorizontal();
                            {
                                EditorGUILayout.LabelField(nameContent);
                                value = EditorGUILayout.TextArea(value, GUILayout.Height(multilineHeight));
                            }
                            EditorGUILayout.EndHorizontal();

                        }
                        else
                        {
                            value = EditorGUILayout.TextField(nameContent, value);
                        }
                    }
                    if (EditorGUI.EndChangeCheck())
                    {
                        fieldInfo.SetValue(data, value);
                    }
                }
                EditorGUI.EndDisabledGroup();

                OnDrawAskOperation();
            }
            EditorGUILayout.EndHorizontal();

        }
    }
}
