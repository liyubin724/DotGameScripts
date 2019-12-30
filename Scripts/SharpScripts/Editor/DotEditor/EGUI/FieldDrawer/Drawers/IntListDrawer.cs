using Rotorz.Games.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace DotEditor.EGUI.FieldDrawer
{
    [TargetFieldType(typeof(List<int>))]
    public class IntListDrawer : AFieldDrawer
    {
        public IntListDrawer(FieldInfo fieldInfo) : base(fieldInfo)
        {
        }

        protected override void OnDraw(object data, bool isShowDesc)
        {
            ReorderableListGUI.Title(fieldInfo.Name);
            List<int> list = (List<int>)fieldInfo.GetValue(data);
            if (list == null)
            {
                EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);
                {
                    EditorGUILayout.LabelField("Data is null");
                    if(GUILayout.Button("New",GUILayout.Width(40)))
                    {
                        list = new List<int>();
                        fieldInfo.SetValue(data, list);
                    }
                }
                EditorGUILayout.EndHorizontal();
            }else
            {
                ReorderableListGUI.ListField<int>(list, (position, value) =>
                {
                    return EditorGUI.IntField(position, value);
                });
            }
        }
    }
}
