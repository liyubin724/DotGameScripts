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

        protected override void OnDraw(bool isShowDesc)
        {
            string value = (string)fieldInfo.GetValue(data);

            EditorGUI.BeginChangeCheck();
            {
                if(isMultilineText)
                {
                    value = EditorGUILayout.TextArea(fieldInfo.Name, value, GUILayout.Height(multilineHeight));
                }else
                {
                    value = EditorGUILayout.TextField(fieldInfo.Name, value);
                }
            }
            if (EditorGUI.EndChangeCheck())
            {
                fieldInfo.SetValue(data, value);
            }
        }
    }
}
