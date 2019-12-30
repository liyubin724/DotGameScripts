using Dot.FieldDrawer;
using Rotorz.Games.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace DotEditor.EGUI.FieldDrawer
{
    [TargetFieldType(typeof(List<string>))]
    public class StringListDrawer : AFieldDrawer
    {
        private bool isMultilineText = false;
        private int multilineHeight = 0;
        public StringListDrawer(FieldInfo fieldInfo) : base(fieldInfo)
        {
            FieldMultilineText attr = fieldInfo.GetCustomAttribute<FieldMultilineText>();
            if (attr != null)
            {
                isMultilineText = true;
                multilineHeight = attr.Height;
            }
        }

        protected override void OnDraw(object data, bool isShowDesc)
        {
            ReorderableListGUI.Title(fieldInfo.Name);
            List<string> list = (List<string>)fieldInfo.GetValue(data);
            if (list == null)
            {
                EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);
                {
                    EditorGUILayout.LabelField("Data is null");
                    if (GUILayout.Button("New", GUILayout.Width(40)))
                    {
                        list = new List<string>();
                        fieldInfo.SetValue(data, list);
                    }
                }
                EditorGUILayout.EndHorizontal();
            }
            else
            {
                if(isMultilineText)
                {
                    ReorderableListGUI.ListField<string>(list, (position, value) =>
                    {
                        return EditorGUI.TextArea(position, value);
                    }, multilineHeight);
                }
                else
                {
                    ReorderableListGUI.ListField<string>(list, (position, value) =>
                    {
                        return EditorGUI.TextField(position, value);
                    });
                }
                
            }
        }
    }
}
